using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using RECAMAS.Domain.Interfaces;
using RECAMAS.Infrastructure.ExternalClients;
using RECAMAS.Infrastructure.Messaging;
using RECAMAS.Infrastructure.Persistence;

namespace RECAMAS.Infrastructure.DependencyInjection;

/// <summary>
/// Registers everything Infrastructure owns: EF Core + Postgres, typed HTTP
/// clients to the 3 reused HTTP-based microservices (Authentication, Storage —
/// Notifications has no HTTP client, see INotificationClient), the Kafka
/// producer used for domain events, and repository implementations as
/// modules get built. Called once from API/Program.cs as
/// services.AddInfrastructureServices(configuration).
/// </summary>
public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // --- PostgreSQL 18, single instance, schema-per-module ---
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("RecamasDb")));

        // --- Reused microservice HTTP clients (Authentication, Storage) ---
        services.AddHttpClient<IAuthenticationClient, AuthenticationClient>(client =>
            {
                client.BaseAddress = new Uri(configuration["Services:Authentication:BaseUrl"]
                    ?? throw new InvalidOperationException("Services:Authentication:BaseUrl not configured"));
            })
            .AddPolicyHandler(GetRetryPolicy());

        services.AddHttpClient<IStorageClient, StorageClient>(client =>
            {
                client.BaseAddress = new Uri(configuration["Services:Storage:BaseUrl"]
                    ?? throw new InvalidOperationException("Services:Storage:BaseUrl not configured"));
            })
            .AddPolicyHandler(GetRetryPolicy());

        // Notifications: no HTTP client registered on purpose — it's Kafka-only, see INotificationClient.

        // --- Kafka producer, shared by every module via IDomainEventPublisher ---
        services.AddSingleton<IProducer<string, string>>(_ =>
        {
            var kafkaConfig = new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"]
                    ?? throw new InvalidOperationException("Kafka:BootstrapServers not configured"),
            };
            return new ProducerBuilder<string, string>(kafkaConfig).Build();
        });
        services.AddSingleton<IDomainEventPublisher, KafkaDomainEventPublisher>();

        // --- Repository implementations, added module by module ---
        // services.AddScoped<ICaseRepository, CaseRepository>();
        // services.AddScoped<ITCNProfileRepository, TCNProfileRepository>();
        // services.AddScoped<IRuleRepository, RuleRepository>();

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromMilliseconds(200 * Math.Pow(2, retryAttempt)));
}
