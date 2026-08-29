using Microsoft.Extensions.Configuration;

namespace RECAMAS.Application.Configuration;

/// <summary>
/// Producer-side only for now — RECAMAS has no Kafka consumer yet. Timeout
/// defaults below match what testing OutboxProcessor against an unreachable
/// broker showed was necessary (see InfrastructureServiceRegistration remarks):
/// librdkafka's own defaults (a 5-minute MessageTimeoutMs) would otherwise
/// leave the outbox retry loop hanging instead of failing fast.
/// </summary>
public class KafkaSettings
{
    public required string BootstrapServers { get; init; }

    public int SocketConnectionSetupTimeoutMs { get; init; } = 10000;
    public int SocketTimeoutMs { get; init; } = 10000;
    public int MessageTimeoutMs { get; init; } = 10000;
    public int RequestTimeoutMs { get; init; } = 10000;

    public static KafkaSettings BindFromConfiguration(IConfiguration configuration)
    {
        return new KafkaSettings
        {
            BootstrapServers = configuration["KAFKA_BOOTSTRAP_SERVERS"]
                ?? throw new InvalidOperationException("KAFKA_BOOTSTRAP_SERVERS is not configured."),
            SocketConnectionSetupTimeoutMs = ParseIntOrDefault(configuration["KAFKA_SOCKET_CONNECTION_SETUP_TIMEOUT_MS"], 10000),
            SocketTimeoutMs = ParseIntOrDefault(configuration["KAFKA_SOCKET_TIMEOUT_MS"], 10000),
            MessageTimeoutMs = ParseIntOrDefault(configuration["KAFKA_MESSAGE_TIMEOUT_MS"], 10000),
            RequestTimeoutMs = ParseIntOrDefault(configuration["KAFKA_REQUEST_TIMEOUT_MS"], 10000),
        };
    }

    private static int ParseIntOrDefault(string? value, int defaultValue) =>
        int.TryParse(value, out var parsed) ? parsed : defaultValue;
}
