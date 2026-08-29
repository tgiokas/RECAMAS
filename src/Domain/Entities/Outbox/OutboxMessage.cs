namespace RECAMAS.Domain.Entities.Outbox;

/// Transactional outbox: written to in the same DbContext/transaction as the
/// business change it describes (or the entity-change diff that produced it),
/// then delivered to Kafka by OutboxProcessor on its own schedule. Guarantees
/// the audit/event record and the underlying change commit or roll back
/// together — the actual point of the pattern (CivilianPortal's own
/// OutboxMessage/OutboxProcessor already does this for its domain events;
/// this is the same mechanism, shared with the automatic entity-change audit
/// trail via the EventType/Category distinction below rather than two
/// parallel delivery pipelines).
///
/// Deliberately NOT a BaseEntity: no soft delete (a processed outbox row
/// should eventually be hard-deleted by a pruning job, not hidden behind
/// IsDeleted — this table is transient delivery plumbing, not the permanent
/// audit record. That record lives in the reused AuditLog service's
/// Elasticsearch index once Kafka delivery succeeds), no PublicId (never
/// exposed across an API boundary), no RowVersion (only OutboxProcessor
/// ever updates a row, so there's no concurrent-edit scenario to guard against).
 
public class OutboxMessage
{
    public long Id { get; set; }

    public Guid EventId { get; set; } = Guid.NewGuid();

    /// e.g. "audit.entity.changed" for auto-captured diffs, or a named business event like "case.created".
    public required string EventType { get; set; }

    /// "Entity" for auto-captured diffs, "Business" for explicit AuditActionService calls, or null for a plain domain event.
    public string? Category { get; set; }

    /// JSON payload published to Kafka as-is (the entity diff, or the explicit event body).
    public required string Payload { get; set; }

    /// Kafka partition key. Defaults to EventId if the producer isn't given one, so ordering isn't accidentally undefined.
    public string? Key { get; set; }

    public string? UserId { get; set; }
    public string? UserName { get; set; }

    public string? EntityType { get; set; }
    public string? EntityId { get; set; }

    public string? CorrelationId { get; set; }

    public DateTimeOffset OccurredAt { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? ProcessedAt { get; set; }
    public DateTimeOffset? LastAttemptAt { get; set; }
    public int AttemptCount { get; set; }

    /// Truncated on write — see OutboxProcessor. Cleared on a successful publish.
    public string? LastError { get; set; }
}
