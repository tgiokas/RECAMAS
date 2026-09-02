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
/// AddHttpClient,TInterface, TImplementation with that BaseUrl, then
/// AddPolicyHandler(GetRetryPolicy()) for transient fault retry. The
/// concrete client itself extends ApiClientBase for structured request
/// response logging and redaction
/// Polly decides whether to retry, ApiClientBase logs whatever
/// actually got sent.
public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // --- PostgreSQL , single instance, schema-per-module ---
        // Interceptors resolved from DI
        services.AddHttpContextAccessor();
        services.AddScoped<AuditColumnsInterceptor>();

        var databaseSettings = DatabaseSettings.BindFromConfiguration(configuration);
        services.AddSingleton(Options.Create(databaseSettings));

        // AddCbsAuditInterceptor resolves AuditSaveChangesInterceptor from DI —
        // registered by AddEntityAuditing<ApplicationDbContext>() in Program.cs.
        // DI resolves against the fully-built container regardless of C# call
        // order, so it doesn't matter that Program.cs's AddCbsAudit(...) call
        // runs "after" AddInfrastructureServices() textually — this lambda only
        // actually executes the first time a scope resolves ApplicationDbContext,
        // by which point every registration in Program.cs has already run.
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
            options.UseNpgsql(databaseSettings.ConnectionString)
                .AddInterceptors(sp.GetRequiredService<AuditColumnsInterceptor>())
                .AddCbsAuditInterceptor(sp));

        // Lets an Application service call SaveChangesAsync without depending on
        // Infrastructure directly
        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

        // --- Typed settings for every outbound HTTP integration ---
        var keycloakSettings = KeycloakSettings.BindFromConfiguration(configuration);
        services.AddSingleton(Options.Create(keycloakSettings));

        var storageSettings = StorageClientSettings.BindFromConfiguration(configuration);
        services.AddSingleton(Options.Create(storageSettings));

        var cyConnectSettings = CyConnectSettings.BindFromConfiguration(configuration);
        services.AddSingleton(Options.Create(cyConnectSettings));

        var arrivalsDeparturesSettings = ArrivalsDeparturesClientSettings.BindFromConfiguration(configuration);
        services.AddSingleton(Options.Create(arrivalsDeparturesSettings));

        var stoplistSettings = StoplistClientSettings.BindFromConfiguration(configuration);
        services.AddSingleton(Options.Create(stoplistSettings));

        var jccSettings = JccClientSettings.BindFromConfiguration(configuration);
        services.AddSingleton(Options.Create(jccSettings));

        // --- Reused microservice HTTP client (Storage) ---
        services.AddHttpClient<IStorageApiClient, StorageApiClient>(client =>
            {
                client.BaseAddress = new Uri(storageSettings.BaseUrl);
            })
            .AddPolicyHandler(GetRetryPolicy());

        // --- External government systems ---
        // ARS and CASS share the CY Connect gateway (Specs 12.3.3) — same BaseUrl, different relative paths.
        services.AddHttpClient<IArsApiClient, ArsApiClient>(client =>
            {
                client.BaseAddress = new Uri(cyConnectSettings.BaseUrl);
            })
            .AddPolicyHandler(GetRetryPolicy());

        services.AddHttpClient<ICassApiClient, CassApiClient>(client =>
            {
                client.BaseAddress = new Uri(cyConnectSettings.BaseUrl);
            })
            .AddPolicyHandler(GetRetryPolicy());

        // PROVISIONAL — see IArrivalsDeparturesApiClient/IStoplistApiClient remarks on the
        // live-API-vs-batch-file contradiction (Specs 9.4/9.5 vs 12.3.6/12.3.7).
        services.AddHttpClient<IArrivalsDeparturesApiClient, ArrivalsDeparturesApiClient>(client =>
            {
                client.BaseAddress = new Uri(arrivalsDeparturesSettings.BaseUrl);
            })
            .AddPolicyHandler(GetRetryPolicy());

        services.AddHttpClient<IStoplistApiClient, StoplistApiClient>(client =>
            {
                client.BaseAddress = new Uri(stoplistSettings.BaseUrl);
            })
            .AddPolicyHandler(GetRetryPolicy());

        services.AddHttpClient<IJccSigningApiClient, JccSigningApiClient>(client =>
            {
                client.BaseAddress = new Uri(jccSettings.BaseUrl);
            })
            .AddPolicyHandler(GetRetryPolicy());

        // FAR: no endpoint exists yet — plain registration, no HttpClient. See IFarClient/FarClient remarks.
        services.AddSingleton<IFarApiClient, FarApiClient>();

        // --- Error catalog, loaded once from errors.json at startup (fail fast if missing) ---
        var errorsJsonPath = Path.Combine(AppContext.BaseDirectory, "errors.json");
        if (!File.Exists(errorsJsonPath))
        {
            throw new FileNotFoundException($"errors.json not found at: {errorsJsonPath}");
        }

        services.AddSingleton(ErrorCatalog.LoadFromFile(errorsJsonPath));

        // --- Repository implementations, added module by module ---
        services.AddScoped<ITCNProfileRepository, TCNProfileRepository>();
        // services.AddScoped<ICaseRepository, CaseRepository>();
        // services.AddScoped<IRuleRepository, RuleRepository>();

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromMilliseconds(200 * Math.Pow(2, retryAttempt)));
}
