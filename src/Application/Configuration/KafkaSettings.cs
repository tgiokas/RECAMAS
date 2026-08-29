using Microsoft.Extensions.Configuration;

namespace RECAMAS.Application.Configuration;

/// Producer-side only for now — RECAMAS has no Kafka consumer yet, so this
/// intentionally omits CivilianPortal's consumer-only fields (GroupId,
/// AutoOffsetReset, SessionTimeoutMs, MaxPollIntervalMs, named topics). Every
/// field that IS here matches CivilianPortal's KafkaSettings/KafkaPublisher
/// (same names, same defaults) so the two codebases share one Kafka-client
/// shape. Timeout defaults were confirmed by testing OutboxProcessor against
/// an unreachable broker (see InfrastructureServiceRegistration remarks):
/// librdkafka's own defaults (a 5-minute MessageTimeoutMs) would otherwise
/// leave the outbox retry loop hanging instead of failing fast.
public class KafkaSettings
{
    public required string BootstrapServers { get; init; }

    public int ReconnectBackoffMs { get; init; } = 100;
    public int ReconnectBackoffMaxMs { get; init; } = 10000;
    public int SocketConnectionSetupTimeoutMs { get; init; } = 10000;
    public int SocketTimeoutMs { get; init; } = 10000;

    public int RetryBackoffMs { get; init; } = 100;
    public int RequestTimeoutMs { get; init; } = 10000;
    public int MessageTimeoutMs { get; init; } = 10000;

    // SASL / TLS — opt-in; all nullable; when absent behaviour is plaintext, identical to before.
    public string? SecurityProtocol { get; init; }
    public string? SaslMechanism { get; init; }
    public string? SaslUsername { get; init; }
    public string? SaslPassword { get; init; }

    public static KafkaSettings BindFromConfiguration(IConfiguration configuration)
    {
        return new KafkaSettings
        {
            BootstrapServers = configuration["KAFKA_BOOTSTRAP_SERVERS"]
                ?? throw new InvalidOperationException("KAFKA_BOOTSTRAP_SERVERS is not configured."),
            ReconnectBackoffMs = ParseIntOrDefault(configuration["KAFKA_RECONNECT_BACKOFF_MS"], 100),
            ReconnectBackoffMaxMs = ParseIntOrDefault(configuration["KAFKA_RECONNECT_BACKOFF_MAX_MS"], 10000),
            SocketConnectionSetupTimeoutMs = ParseIntOrDefault(configuration["KAFKA_SOCKET_CONNECTION_SETUP_TIMEOUT_MS"], 10000),
            SocketTimeoutMs = ParseIntOrDefault(configuration["KAFKA_SOCKET_TIMEOUT_MS"], 10000),
            RetryBackoffMs = ParseIntOrDefault(configuration["KAFKA_RETRY_BACKOFF_MS"], 100),
            RequestTimeoutMs = ParseIntOrDefault(configuration["KAFKA_REQUEST_TIMEOUT_MS"], 10000),
            MessageTimeoutMs = ParseIntOrDefault(configuration["KAFKA_MESSAGE_TIMEOUT_MS"], 10000),
            SecurityProtocol = configuration["KAFKA_SECURITY_PROTOCOL"],
            SaslMechanism = configuration["KAFKA_SASL_MECHANISM"],
            SaslUsername = configuration["KAFKA_SASL_USERNAME"],
            SaslPassword = configuration["KAFKA_SASL_PASSWORD"],
        };
    }

    private static int ParseIntOrDefault(string? value, int defaultValue) =>
        int.TryParse(value, out var parsed) ? parsed : defaultValue;
}
