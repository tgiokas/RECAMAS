using System.Text;
using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RECAMAS.Application.Configuration;
using RECAMAS.Application.Interfaces;

namespace RECAMAS.Infrastructure.Messaging;

/// Ported from CivilianPortal's KafkaPublisher (same shape, same producer
/// options) so both codebases share one Kafka-client pattern instead of
/// RECAMAS holding a bare IProducer&lt;string, string&gt; directly. Registered
/// as a singleton — the producer itself is thread-safe and meant to be
/// long-lived, so IDisposable flushes/closes it once, at app shutdown.
public sealed class KafkaPublisher : IMessagePublisher, IDisposable
{
    private readonly IProducer<string, string> _producer;
    private readonly ILogger<KafkaPublisher> _logger;

    public KafkaPublisher(IOptions<KafkaSettings> kafkaOptions, ILogger<KafkaPublisher> logger)
    {
        _logger = logger;
        var settings = kafkaOptions.Value;

        var producerConfig = new ProducerConfig
        {
            BootstrapServers = settings.BootstrapServers,
            ReconnectBackoffMs = settings.ReconnectBackoffMs,
            ReconnectBackoffMaxMs = settings.ReconnectBackoffMaxMs,
            SocketConnectionSetupTimeoutMs = settings.SocketConnectionSetupTimeoutMs,
            SocketTimeoutMs = settings.SocketTimeoutMs,

            EnableIdempotence = true,
            Acks = Acks.All,
            RetryBackoffMs = settings.RetryBackoffMs,
            RequestTimeoutMs = settings.RequestTimeoutMs,
            MessageTimeoutMs = settings.MessageTimeoutMs,
        };
        producerConfig.ApplySasl(settings);

        _producer = new ProducerBuilder<string, string>(producerConfig).Build();

        _logger.LogInformation(
            "Kafka producer initialized — Servers: {Servers}, Acks: {Acks}, Idempotent: {Idempotent}",
            settings.BootstrapServers, Acks.All.ToString(), true);
    }

    public async Task PublishJsonAsync<T>(
        string route,
        string key,
        T payload,
        IEnumerable<KeyValuePair<string, string>>? headers = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var json = JsonSerializer.Serialize(payload);

            var msg = new Message<string, string>
            {
                Key = key ?? string.Empty,
                Value = json,
                Headers = new Headers(),
            };

            if (headers is not null)
            {
                foreach (var h in headers)
                    msg.Headers.Add(h.Key, Encoding.UTF8.GetBytes(h.Value));
            }

            var result = await _producer.ProduceAsync(route, msg, cancellationToken);
            _logger.LogDebug("Produced to {TP} (offset {Offset})", result.TopicPartition, result.Offset);
        }
        catch (ProduceException<string, string> ex)
        {
            _logger.LogError(ex, "Kafka produce error: {Reason}", ex.Error.Reason);
            throw;
        }
    }

    public async Task PublishRawJsonAsync(
        string route,
        string key,
        string jsonPayload,
        IEnumerable<KeyValuePair<string, string>>? headers = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var msg = new Message<string, string>
            {
                Key = key ?? string.Empty,
                Value = jsonPayload, // used as-is, no re-serialization
                Headers = new Headers(),
            };

            if (headers is not null)
            {
                foreach (var h in headers)
                    msg.Headers.Add(h.Key, Encoding.UTF8.GetBytes(h.Value));
            }

            var result = await _producer.ProduceAsync(route, msg, cancellationToken);
            _logger.LogDebug("Produced to {TP} (offset {Offset})", result.TopicPartition, result.Offset);
        }
        catch (ProduceException<string, string> ex)
        {
            _logger.LogError(ex, "Kafka produce error: {Reason}", ex.Error.Reason);
            throw;
        }
    }

    public void Dispose()
    {
        try { _producer.Flush(TimeSpan.FromSeconds(5)); }
        catch (Exception ex) { _logger.LogWarning(ex, "Error flushing Kafka producer during dispose"); }
        finally { _producer.Dispose(); }
    }
}
