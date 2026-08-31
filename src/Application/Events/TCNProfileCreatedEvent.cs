namespace RECAMAS.Application.Events;

/// Integration event staged to the outbox when a new TCN Profile is
/// registered, for Notifications/AuditLog (or any future Kafka consumer) to
/// react to — they consume this instead of polling the database. Written
/// directly by TCNProfileService via IOutboxRepository.AddWithoutSaveAsync
/// rather than through IDomainEventPublisher — see TCNProfileService remarks.
public sealed record TCNProfileCreatedEvent(Guid TCNProfilePublicId, string? Arc, string? FirstNameEn, string? LastNameEn);
