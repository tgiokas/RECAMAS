using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Polly;
using Polly.Extensions.Http;
using Cbs.Audit.DependencyInjection;

using RECAMAS.Application.Configuration;
using RECAMAS.Application.Errors;
using RECAMAS.Application.Interfaces;
using RECAMAS.Domain.Interfaces;
using RECAMAS.Infrastructure.ExternalClients;
using RECAMAS.Infrastructure.Database;
using RECAMAS.Infrastructure.Database.Interceptors;
using RECAMAS.Infrastructure.Repositories;

namespace RECAMAS.Infrastructure;

/// Registers everything Infrastructure owns: EF Core + Postgres, typed HTTP
/// clients (both the reused Storage microservice and the external government
/// systems), the error catalog, and repository implementations as modules get
/// built. Called once from API/Program.cs as
/// services.AddInfrastructureServices(configuration). Cbs.Audit's own DI
/// registration (AddCbsAudit/AddEntityAuditing/etc.) is wired directly in
/// Program.cs instead of here, matching the auditing doc's own placement.
///
/// Every HTTP client here is registered the same way: bind its typed settings,
/// AddHttpClient,TInterface, TImplementation with that BaseUrl and a per-request
/// Timeout, then AddPolicyHandler(GetRetryPolicy()) for transient-fault retry
/// followed by AddPolicyHandler(GetCircuitBreakerPolicy()) so a system that's
/// actually down fails fast instead of being retried on every call — this
/// matters because Specs section 9 has ARS/CASS/Arrivals-Departures/Stoplist
/// called in a sequential chain per TCN Search, again on every profile/case/
/// implementation open, and again in a daily batch across every TCN with an
/// open case, so a slow or down system left to the 100s BCL default timeout
/// and bare retry would compound badly across all three call sites.
/// Polly decides whether to retry/break, ApiClientBase logs whatever actually got sent.
public static class InfrastructureServiceRegistration
{
    // Government interfaces run over CY Connect / the Police Public Zone —
    // internal-network hops, not the public internet — so a slow response is a
    // signal something is wrong, not normal latency. Kept short deliberately:
    // callers (TCN Search, the per-open refresh, the daily batch) all need a
    // single stuck system to fail fast rather than hold up the whole chain.
    private static readonly TimeSpan ExternalSystemTimeout = TimeSpan.FromSeconds(15);

    // Storage is our own reused microservice, not a government interface, and
    // handles file uploads — allowed a bit more headroom.
    private static readonly TimeSpan StorageTimeout = TimeSpan.FromSeconds(30);

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // --- Typed settings for every outbound HTTP integration ---
        var keycloakSettings = KeycloakSettings.BindFromConfiguration(configuration);
        services.AddSingleton(Options.Create(keycloakSettings));

        var storageSettings = StorageApiClientSettings.BindFromConfiguration(configuration);
        services.AddSingleton(Options.Create(storageSettings));

        var cyConnectSettings = CyConnectSettings.BindFromConfiguration(configuration);
        services.AddSingleton(Options.Create(cyConnectSettings));

        var arrivalsDeparturesSettings = ArrivalsDeparturesApiClientSettings.BindFromConfiguration(configuration);
        services.AddSingleton(Options.Create(arrivalsDeparturesSettings));

        var stoplistSettings = StoplistApiClientSettings.BindFromConfiguration(configuration);
        services.AddSingleton(Options.Create(stoplistSettings));

        var jccSettings = JccApiClientSettings.BindFromConfiguration(configuration);
        services.AddSingleton(Options.Create(jccSettings));

        // --- PostgreSQL , single instance, schema-per-module ---
        // Interceptors resolved from DI
        services.AddHttpContextAccessor();
        services.AddScoped<AuditColumnsInterceptor>();

        var databaseSettings = DatabaseSettings.BindFromConfiguration(configuration);
        services.AddSingleton(Options.Create(databaseSettings));

        // AddCbsAuditInterceptor resolves AuditSaveChangesInterceptor from DI —
        // registered by AddEntityAuditing<ApplicationDbContext>() in Program.cs.
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
            options.UseNpgsql(databaseSettings.ConnectionString)
                .AddInterceptors(sp.GetRequiredService<AuditColumnsInterceptor>())
                .AddCbsAuditInterceptor(sp));

        // Lets an Application service call SaveChangesAsync without depending on
        // Infrastructure directly
        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

        // --- Reused microservice HTTP client (Storage) ---
        services.AddHttpClient<IStorageApiClient, StorageApiClient>(client =>
            {
                client.BaseAddress = new Uri(storageSettings.BaseUrl);
                client.Timeout = StorageTimeout;
            })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        // --- External government systems ---
        // ARS and CASS share the CY Connect gateway (Specs 12.3.3) — same BaseUrl, different relative paths.
        services.AddHttpClient<IArsApiClient, ArsApiClient>(client =>
            {
                client.BaseAddress = new Uri(cyConnectSettings.BaseUrl);
                client.Timeout = ExternalSystemTimeout;
            })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        services.AddHttpClient<ICassApiClient, CassApiClient>(client =>
            {
                client.BaseAddress = new Uri(cyConnectSettings.BaseUrl);
                client.Timeout = ExternalSystemTimeout;
            })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        // PROVISIONAL — see IArrivalsDeparturesApiClient/IStoplistApiClient remarks on the
        // live-API-vs-batch-file contradiction (Specs 9.4/9.5 vs 12.3.6/12.3.7).
        services.AddHttpClient<IArrivalsDeparturesApiClient, ArrivalsDeparturesApiClient>(client =>
            {
                client.BaseAddress = new Uri(arrivalsDeparturesSettings.BaseUrl);
                client.Timeout = ExternalSystemTimeout;
            })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        services.AddHttpClient<IStoplistApiClient, StoplistApiClient>(client =>
            {
                client.BaseAddress = new Uri(stoplistSettings.BaseUrl);
                client.Timeout = ExternalSystemTimeout;
            })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        services.AddHttpClient<IJccSigningApiClient, JccSigningApiClient>(client =>
            {
                client.BaseAddress = new Uri(jccSettings.BaseUrl);
                client.Timeout = ExternalSystemTimeout;
            })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        // FAR: no endpoint exists yet — plain registration, no HttpClient. See IFarClient/FarClient remarks.
        services.AddSingleton<IFarApiClient, FarApiClient>();

        // --- Error catalog, loaded once from errors.json at startup (fail fast if missing) ---
        // Add Error Catalog Path
        var path = Path.Combine(Environment.CurrentDirectory, "errors.json");
        if (!File.Exists(path))
            throw new FileNotFoundException($"errors.json not found at: {path}");

        var errorcat = ErrorCatalog.LoadFromFile(path);
        services.AddSingleton<IErrorCatalog>(errorcat);

        // --- Repository implementations, added module by module ---
        services.AddScoped<ITCNProfileRepository, TCNProfileRepository>();
        // services.AddScoped<ICaseRepository, CaseRepository>();
        // services.AddScoped<IRuleRepository, RuleRepository>();

        return services;
    }

    // Exponential backoff (200/400/800ms) plus up to 100ms of jitter, so that
    // many concurrent callers retrying at once (e.g. the daily refresh batch
    // hitting the same degraded system for many TCNs) don't all land on the
    // exact same retry instant and pile onto it together.
    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt =>
                TimeSpan.FromMilliseconds(200 * Math.Pow(2, retryAttempt))
                + TimeSpan.FromMilliseconds(Random.Shared.Next(0, 100)));

    // Must be called once per AddHttpClient<T> registration (as above), never
    // shared across clients — Polly's circuit-breaker policy is stateful, so a
    // single shared instance would trip every external system's circuit the
    // moment any one of them (e.g. CASS) failed 5 times, instead of isolating
    // the failure to that system. Opens after 5 consecutive transient
    // failures and stays open 30s before allowing a single probe request
    // through; while open, calls fail immediately (ApiClientBase's catch-all
    // turns the resulting BrokenCircuitException into the same synthetic 503
    // callers already handle) instead of waiting out ExternalSystemTimeout
    // and retrying 3 more times per call.
    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(handledEventsAllowedBeforeBreaking: 5, durationOfBreak: TimeSpan.FromSeconds(30));
}
