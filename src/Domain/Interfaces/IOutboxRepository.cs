using RECAMAS.Domain.Entities.Outbox;

namespace RECAMAS.Domain.Interfaces;

/// <summary>Same shape as CivilianPortal's IOutboxRepository — see its remarks for why writes there skip SaveChanges.</summary>
public interface IOutboxRepository
{
    Task<List<OutboxMessage>> GetPendingAsync(int batchSize, int maxAttempts, CancellationToken ct = default);

    /// <summary>No SaveChanges — caller commits the transaction alongside whatever change this message describes.</summary>
    Task AddWithoutSaveAsync(OutboxMessage message, CancellationToken ct = default);

    /// <summary>Adds and commits immediately — for standalone publishes with no other pending change to commit alongside.</summary>
    Task AddAsync(OutboxMessage message, CancellationToken ct = default);

    Task MarkAsProcessedAsync(long id, CancellationToken ct = default);

    Task MarkAsFailedAsync(long id, string error, CancellationToken ct = default);
}
