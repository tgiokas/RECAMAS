using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using RECAMAS.Application.Configuration;
using RECAMAS.Application.Errors;
using RECAMAS.Application.Interfaces;
using RECAMAS.Domain.Interfaces;
using RECAMAS.Infrastructure.ExternalClients;
using RECAMAS.Infrastructure.Messaging;
using RECAMAS.Infrastructure.Persistence;
using RECAMAS.Infrastructure.Persistence.Interceptors;
using RECAMAS.Infrastructure.Repositories;

namespace RECAMAS.Infrastructure.DependencyInjection;

/// Registers everything Infrastructure owns: EF Core + Postgres, typed HTTP
/// clients (both the reused Storage microservice and the external government
/// systems), the Kafka producer used for domain events, the error catalog,
/// and repository implementations as modules get built. Called once from
/// API/Program.cs as services.AddInfrastructureServices(configuration).
///
/// Every HTTP client here is registered the same way: bind its typed settings,
/// AddHttpClient&lt;TInterface, TImplementation&gt; with that BaseUrl, then
/// AddPolicyHandler(GetRetryPolicy()) for transient-fault retry. The
/// concrete client itself extends ApiClientBase for structured request/
/// response logging and redaction — the two are complementary, not
/// alternatives: Polly decides whether to retry, ApiClientBase logs whatever
/// actually got sent.
public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // --- PostgreSQL 18, single instance, schema-per-module ---
        // Interceptors resolved from DI 
        services.AddHttpContextAccessor();
        services.AddScoped<AuditColumnsInterceptor>();
        services.AddScoped<EntityChangeAuditInterceptor>();

        var databaseSettings = DatabaseSettings.BindFromConfiguration(configuration);
        services.AddSingleton(Options.Create(databaseSettings));

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
            options.UseNpgsql(databaseSettings.ConnectionString)
                .AddInterceptors(
                    sp.GetRequiredService<AuditColumnsInterceptor>(),
                    sp.GetRequiredService<EntityChangeAuditInterceptor>()));

        // Lets an Application service call SaveChangesAsync without depending on
        // Infrastructure directly — see IApplicationDbContext remarks.
        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

        // --- Typed settings for every outbound HTTP integration ---
        var keycloakSettings = KeycloakSettings.BindFromConfiguration(configuration);
        services.AddSingleton(Options.Create(keycloakSettings));

        var kafkaSettings = KafkaSettings.BindFromConfiguration(configuration);
        services.AddSingleton(Options.Create(kafkaSettings));

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
        services.AddHttpClient<IStorageClient, StorageClient>(client =>
            {
                client.BaseAddress = new Uri(storageSettings.BaseUrl);
            })
            .AddPolicyHandler(GetRetryPolicy()); 

        // --- External government systems ---
        // ARS and CASS share the CY Connect gateway (Specs 12.3.3) — same BaseUrl, different relative paths.
        services.AddHttpClient<IArsClient, ArsClient>(client =>
            {
                client.BaseAddress = new Uri(cyConnectSettings.BaseUrl);
            })
            .AddPolicyHandler(GetRetryPolicy());

        services.AddHttpClient<ICassClient, CassClient>(client =>
            {
                client.BaseAddress = new Uri(cyConnectSettings.BaseUrl);
            })
            .AddPolicyHandler(GetRetryPolicy());

        // PROVISIONAL — see IArrivalsDeparturesClient/IStoplistClient remarks on the
        // live-API-vs-batch-file contradiction (Specs 9.4/9.5 vs 12.3.6/12.3.7).
        services.AddHttpClient<IArrivalsDeparturesClient, ArrivalsDeparturesClient>(client =>
            {
                client.BaseAddress = new Uri(arrivalsDeparturesSettings.BaseUrl);
            })
            .AddPolicyHandler(GetRetryPolicy());

        services.AddHttpClient<IStoplistClient, StoplistClient>(client =>
            {
                client.BaseAddress = new Uri(stoplistSettings.BaseUrl);
            })
            .AddPolicyHandler(GetRetryPolicy());

        services.AddHttpClient<IJccSigningClient, JccSigningClient>(client =>
            {
                client.BaseAddress = new Uri(jccSettings.BaseUrl);
            })
            .AddPolicyHandler(GetRetryPolicy());

        // FAR: no endpoint exists yet — plain registration, no HttpClient. See IFarClient/FarClient remarks.
        services.AddSingleton<IFarClient, FarClient>();

        // --- Kafka producer, shared by every module via IDomainEventPublisher ---
        // Same IMessagePublisher/KafkaPublisher pattern as CivilianPortal (see
        // KafkaPublisher's own remarks) instead of callers holding a bare
        // IProducer&lt;string, string&gt; directly.
        services.AddSingleton<IMessagePublisher, KafkaPublisher>();
        // Scoped, not Singleton: OutboxDomainEventPublisher depends (via IOutboxRepository)
        // on the scoped ApplicationDbContext.
        services.AddScoped<IDomainEventPublisher, OutboxDomainEventPublisher>();
        services.AddScoped<IAuditActionService, AuditActionService>();
        services.AddHostedService<OutboxProcessor>();

        // --- Error catalog, loaded once from errors.json at startup (fail fast if missing) ---
        var errorsJsonPath = Path.Combine(AppContext.BaseDirectory, "errors.json");
        if (!File.Exists(errorsJsonPath))
        {
            throw new FileNotFoundException($"errors.json not found at: {errorsJsonPath}");
        }

        services.AddSingleton(ErrorCatalog.LoadFromFile(errorsJsonPath));

        // --- Repository implementations, added module by module ---
        services.AddScoped<ITCNProfileRepository, TCNProfileRepository>();
        services.AddScoped<IOutboxRepository, OutboxRepository>();
        // services.AddScoped<ICaseRepository, CaseRepository>();
        // services.AddScoped<IRuleRepository, RuleRepository>();

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromMilliseconds(200 * Math.Pow(2, retryAttempt)));
}
