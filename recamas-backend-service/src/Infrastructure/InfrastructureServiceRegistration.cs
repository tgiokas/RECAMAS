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
/// Every HTTP client here is registered the same way: typed settings, a
/// per-request Timeout, then retry + circuit-breaker policies — so a down
/// external system (called repeatedly per Specs section 9: TCN Search,
/// every profile/case/implementation open, and the daily refresh) fails
/// fast instead of compounding via the 100s default timeout and bare retry.
public static class InfrastructureServiceRegistration
{
    private static readonly TimeSpan ExternalSystemTimeout = TimeSpan.FromSeconds(15);
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

        // Add CbsAuditInterceptor resolves AuditSaveChangesInterceptor from DI —
        // registered by AddEntityAuditing<ApplicationDbContext>() in Program.cs.
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
            options.UseNpgsql(databaseSettings.ConnectionString)
                .AddInterceptors(sp.GetRequiredService<AuditColumnsInterceptor>())
                .AddCbsAuditInterceptor(sp));

        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

        // --- CBS microservice HTTP client (Storage) ---
        services.AddHttpClient<IStorageApiClient, StorageApiClient>(client =>
            {
                client.BaseAddress = new Uri(storageSettings.BaseUrl);
                client.Timeout = StorageTimeout;
            })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        // --- External RECAMAS systems ---
        // ARS 
        services.AddHttpClient<IArsApiClient, ArsApiClient>(client =>
            {
                client.BaseAddress = new Uri(cyConnectSettings.BaseUrl);
                client.Timeout = ExternalSystemTimeout;
            })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());
        
        // CASS
        services.AddHttpClient<ICassApiClient, CassApiClient>(client =>
            {
                client.BaseAddress = new Uri(cyConnectSettings.BaseUrl);
                client.Timeout = ExternalSystemTimeout;
            })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        //Arrivals/Departures
        services.AddHttpClient<IArrivalsDeparturesApiClient, ArrivalsDeparturesApiClient>(client =>
            {
                client.BaseAddress = new Uri(arrivalsDeparturesSettings.BaseUrl);
                client.Timeout = ExternalSystemTimeout;
            })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());
        
        // Stoplist
        services.AddHttpClient<IStoplistApiClient, StoplistApiClient>(client =>
            {
                client.BaseAddress = new Uri(stoplistSettings.BaseUrl);
                client.Timeout = ExternalSystemTimeout;
            })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());
        
        // JCC Signing
        services.AddHttpClient<IJccSigningApiClient, JccSigningApiClient>(client =>
            {
                client.BaseAddress = new Uri(jccSettings.BaseUrl);
                client.Timeout = ExternalSystemTimeout;
            })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        // FAR: no endpoint exists yet
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

    // Exponential backoff (200/400/800ms) + up to 100ms jitter, so concurrent
    // callers don't retry in lockstep against the same degraded system.
    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt =>
                TimeSpan.FromMilliseconds(200 * Math.Pow(2, retryAttempt))
                + TimeSpan.FromMilliseconds(Random.Shared.Next(0, 100)));

    // Call once per AddHttpClient<T> (never shared/reused) — Polly's circuit
    // state is stateful, so sharing one instance would trip every system's
    // circuit off one system's failures. Opens after 5 consecutive failures,
    // stays open 30s, then allows a single probe through.
    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(handledEventsAllowedBeforeBreaking: 5, durationOfBreak: TimeSpan.FromSeconds(30));
}
