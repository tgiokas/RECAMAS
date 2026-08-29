namespace RECAMAS.Domain.Interfaces;

/// Every module publishes domain events through this single interface
/// (e.g. "case created", "case stage changed", "detention order issued").
/// One publish, two independent consumers on the other side of Kafka:
/// Notifications (reused) turns some events into email/SMS, AuditLog (reused)
/// indexes every event into Elasticsearch for compliance/audit trail.
///
/// RECAMAS never calls either of those services directly for this — it only
/// ever writes to Kafka. Implemented in Infrastructure/Messaging/KafkaDomainEventPublisher.cs.
///
/// OPEN ITEM: exact topic name/event schema AuditLog expects is still pending
/// (see architecture decision log) — this interface's shape may need to change
/// once that's confirmed.
public interface IDomainEventPublisher
{
    Task PublishAsync<TEvent>(TEvent domainEvent, CancellationToken ct = default) where TEvent : class;
}
