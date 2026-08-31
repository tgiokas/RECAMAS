namespace RECAMAS.Application.Interfaces;

/// Same seam CivilianPortal's IApplicationDbContext provides — lets an
/// Application service commit repository writes staged via AddWithoutSaveAsync
/// without depending on Infrastructure (ApplicationDbContext lives there).
/// Deliberately narrower than CivilianPortal's version: no BeginTransactionAsync,
/// since that returns Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction,
/// which would pull an EF Core package reference into Application — this project's
/// own csproj comment says Application depends on Domain only. Not needed yet
/// anyway: a single SaveChangesAsync call already commits every staged write
/// in one implicit transaction, which is all the outbox pattern requires.
public interface IApplicationDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
