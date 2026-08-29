using System.Text.Json;
using Microsoft.Extensions.Logging;
using RECAMAS.Domain.Entities.Outbox;
using RECAMAS.Domain.Interfaces;

namespace RECAMAS.Infrastructure.Messaging;

/// 
/// Replaces the previous direct-to-Kafka implementation (KafkaDomainEventPublisher)
/// with a transactional-outbox write — closing the reliability gap that class's
/// own doc comment already flagged ("publishing failure should never fail the
/// calling business operation — log and move on"). Silently dropping an event on
/// a Kafka outage is exactly what the outbox pattern exists to prevent: the row
/// survives in Postgres and OutboxProcessor retries it until Kafka is back.
///
/// Stages the outbox row and commits it immediately via IOutboxRepository.AddAsync
/// — NOT automatically atomic with any other pending change on the ambient
/// DbContext. That's a deliberate scope limit: true same-transaction atomicity
/// with a specific business write needs IOutboxRepository.AddWithoutSaveAsync
/// called directly by that write's own repository, not through this generic
/// publisher. The automatic entity-diff audit trail (EntityChangeAuditInterceptor)
/// gets the strong guarantee this way instead, piggybacking on whatever
/// SaveChanges the caller was already doing for its own entity change.
///
/// OPEN ITEM (carried over): exact event-type naming/schema AuditLog and
/// Notifications expect is still unconfirmed.
/// 
public class OutboxDomainEventPublisher : IDomainEventPublisher
{
    private readonly IOutboxRepository _outboxRepository;
    private readonly ILogger<OutboxDomainEventPublisher> _logger;

    public OutboxDomainEventPublisher(IOutboxRepository outboxRepository, ILogger<OutboxDomainEventPublisher> logger)
    {
        _outboxRepository = outboxRepository;
        _logger = logger;
    }

    public async Task PublishAsync<TEvent>(TEvent domainEvent, CancellationToken ct = default) where TEvent : class
    {
        var message = new OutboxMessage
        {
            EventType = typeof(TEvent).Name,
            Payload = JsonSerializer.Serialize(domainEvent),
        };

        await _outboxRepository.AddAsync(message, ct);

        _logger.LogDebug("Staged domain event {EventType} ({EventId}) in the outbox", message.EventType, message.EventId);
    }
}
