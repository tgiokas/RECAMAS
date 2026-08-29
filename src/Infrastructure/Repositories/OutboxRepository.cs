using Microsoft.EntityFrameworkCore;
using RECAMAS.Domain.Entities.Outbox;
using RECAMAS.Domain.Interfaces;
using RECAMAS.Infrastructure.Persistence;

namespace RECAMAS.Infrastructure.Repositories;

public class OutboxRepository : IOutboxRepository
{
    private readonly ApplicationDbContext _dbContext;

    public OutboxRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<OutboxMessage>> GetPendingAsync(int batchSize, int maxAttempts, CancellationToken ct = default)
    {
        return await _dbContext.OutboxMessages
            .AsNoTracking()
            .Where(m => m.ProcessedAt == null && m.AttemptCount < maxAttempts)
            .OrderBy(m => m.OccurredAt)
            .Take(batchSize)
            .ToListAsync(ct);
    }

    // No SaveChanges, caller commits the transaction alongside the change this message describes.
    public async Task AddWithoutSaveAsync(OutboxMessage message, CancellationToken ct = default)
    {
        await _dbContext.OutboxMessages.AddAsync(message, ct);
    }

    public async Task AddAsync(OutboxMessage message, CancellationToken ct = default)
    {
        await _dbContext.OutboxMessages.AddAsync(message, ct);
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task MarkAsProcessedAsync(long id, CancellationToken ct = default)
    {
        await _dbContext.OutboxMessages
            .Where(m => m.Id == id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(m => m.ProcessedAt, DateTimeOffset.UtcNow)
                .SetProperty(m => m.LastError, (string?)null), ct);
    }

    public async Task MarkAsFailedAsync(long id, string error, CancellationToken ct = default)
    {
        // Truncated outside the expression tree — ExecuteUpdateAsync's lambda can't contain a range expression.
        var truncatedError = error.Length > 2000 ? error.Substring(0, 2000) : error;

        await _dbContext.OutboxMessages
            .Where(m => m.Id == id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(m => m.AttemptCount, m => m.AttemptCount + 1)
                .SetProperty(m => m.LastAttemptAt, DateTimeOffset.UtcNow)
                .SetProperty(m => m.LastError, truncatedError), ct);
    }
}
