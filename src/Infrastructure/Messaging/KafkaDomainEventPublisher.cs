using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using RECAMAS.Domain.Interfaces;

namespace RECAMAS.Infrastructure.Messaging;

/// <summary>
/// Single Kafka producer used by every module to publish domain events.
/// Notifications (reused) and AuditLog (reused) both consume independently —
/// this class knows nothing about either of them, it just publishes.
///
/// OPEN ITEM: topic name below is a placeholder. Needs to be confirmed against
/// what AuditLog actually expects to consume, and whether we extend Notifications'
/// existing notifications.email.{auth|backend|citizen} pattern or add a dedicated
/// notifications.email.recamas / recamas.audit.events topic.
/// </summary>
public class KafkaDomainEventPublisher : IDomainEventPublisher, IDisposable
{
    private const string PlaceholderTopic = "recamas.domain.events"; // TODO: confirm real topic name(s)

    private readonly IProducer<string, string> _producer;
    private readonly ILogger<KafkaDomainEventPublisher> _logger;

    public KafkaDomainEventPublisher(IProducer<string, string> producer, ILogger<KafkaDomainEventPublisher> logger)
    {
        _producer = producer;
        _logger = logger;
    }

    public async Task PublishAsync<TEvent>(TEvent domainEvent, CancellationToken ct = default) where TEvent : class
    {
        var payload = JsonSerializer.Serialize(domainEvent);
        var message = new Message<string, string>
        {
            Key = typeof(TEvent).Name,
            Value = payload,
        };

        try
        {
            await _producer.ProduceAsync(PlaceholderTopic, message, ct);
        }
        catch (ProduceException<string, string> ex)
        {
            // Publishing failure should never fail the calling business operation —
            // log and move on. Revisit once we decide if any event is critical-path
            // (e.g. should a failed audit publish block case creation? Almost certainly not).
            _logger.LogError(ex, "Failed to publish {EventType} to Kafka topic {Topic}", typeof(TEvent).Name, PlaceholderTopic);
        }
    }

    public void Dispose() => _producer?.Dispose();
}
