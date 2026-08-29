using RECAMAS.Domain.Entities.Outbox;

namespace RECAMAS.Domain.Interfaces;

/// Same shape as CivilianPortal's IOutboxRepository — see its remarks for why writes there skip SaveChanges.
public interface IOutboxRepository
{
    Task<List<OutboxMessage>> GetPendingAsync(int batchSize, int maxAttempts, CancellationToken ct = default);

    /// No SaveChanges — caller commits the transaction alongside whatever change this message describes.
    Task AddWithoutSaveAsync(OutboxMessage message, CancellationToken ct = default);

    /// Adds and commits immediately — for standalone publishes with no other pending change to commit alongside.
    Task AddAsync(OutboxMessage message, CancellationToken ct = default);

    Task MarkAsProcessedAsync(long id, CancellationToken ct = default);

    Task MarkAsFailedAsync(long id, string error, CancellationToken ct = default);
}
