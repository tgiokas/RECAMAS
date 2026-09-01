using Microsoft.EntityFrameworkCore;
using RECAMAS.Domain.Interfaces;
using RECAMAS.Infrastructure.Database;
using TCNProfileEntity = RECAMAS.Domain.Entities.TCNProfile.TCNProfile;

namespace RECAMAS.Infrastructure.Repositories;

public class TCNProfileRepository : ITCNProfileRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TCNProfileRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TCNProfileEntity?> GetByIdWithDetailsAsync(long id, CancellationToken ct = default)
    {
        return await FullGraph(_dbContext.TCNProfiles.AsNoTracking())
            .FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task<TCNProfileEntity?> GetByPublicIdWithDetailsAsync(Guid publicId, CancellationToken ct = default)
    {
        return await FullGraph(_dbContext.TCNProfiles.AsNoTracking())
            .FirstOrDefaultAsync(p => p.PublicId == publicId, ct);
    }

    public async Task<TCNProfileEntity?> GetByIdForUpdateAsync(long id, CancellationToken ct = default)
    {
        return await FullGraph(_dbContext.TCNProfiles)
            .FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task<TCNProfileEntity?> GetByArcAsync(string arc, CancellationToken ct = default)
    {
        return await _dbContext.TCNProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Arc == arc, ct);
    }

    public async Task<IReadOnlyList<TCNProfileEntity>> SearchForDuplicatesAsync(
        string? arc, string? passportNumber, string? firstName, string? lastName, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(arc) && string.IsNullOrWhiteSpace(passportNumber)
            && string.IsNullOrWhiteSpace(firstName) && string.IsNullOrWhiteSpace(lastName))
        {
            return [];
        }

        var query = _dbContext.TCNProfiles
            .AsNoTracking()
            .Include(p => p.IdentityDocuments)
            .Include(p => p.Nationalities)
            .AsSplitQuery()
            .AsQueryable();

        query = query.Where(p =>
            (!string.IsNullOrWhiteSpace(arc) && p.Arc == arc) ||
            (!string.IsNullOrWhiteSpace(passportNumber) && p.IdentityDocuments.Any(d => d.DocumentNumber == passportNumber)) ||
            (!string.IsNullOrWhiteSpace(firstName) && EF.Functions.TrigramsAreSimilar(p.FirstNameEn ?? "", firstName)) ||
            (!string.IsNullOrWhiteSpace(lastName) && EF.Functions.TrigramsAreSimilar(p.LastNameEn ?? "", lastName)));

        return await query.ToListAsync(ct);
    }

    public async Task<(IReadOnlyList<TCNProfileEntity> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, string? quickSearchTerm, CancellationToken ct = default)
    {
        var query = _dbContext.TCNProfiles.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(quickSearchTerm))
        {
            query = query.Where(p =>
                p.Arc == quickSearchTerm ||
                p.DisplayCode == quickSearchTerm ||
                EF.Functions.TrigramsAreSimilar(p.FirstNameEn ?? "", quickSearchTerm) ||
                EF.Functions.TrigramsAreSimilar(p.LastNameEn ?? "", quickSearchTerm));
        }

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip(Math.Max(0, page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }

    // No SaveChanges, caller commits the transaction (outbox pattern).
    public async Task AddWithoutSaveAsync(TCNProfileEntity profile, CancellationToken ct = default)
    {
        await _dbContext.TCNProfiles.AddAsync(profile, ct);
    }

    public async Task UpdateAsync(TCNProfileEntity profile, CancellationToken ct = default)
    {
        await _dbContext.SaveChangesAsync(ct);
    }

    private static IQueryable<TCNProfileEntity> FullGraph(IQueryable<TCNProfileEntity> query)
    {
        // AsSplitQuery is mandatory here: 12 sibling collections in a single-query
        // JOIN would cartesian-product against each other (EF Core's own
        // MultipleCollectionIncludeWarning, confirmed at runtime while testing this).
        return query
            .AsSplitQuery()
            .Include(p => p.Nationalities)
            .Include(p => p.IdentityDocuments)
            .Include(p => p.ResidencyStatuses)
            .Include(p => p.ResidencyApplications)
            .Include(p => p.InternationalProtectionStatuses)
            .Include(p => p.InternationalProtectionApplications)
            .Include(p => p.Appeals)
            .Include(p => p.ReturnDecisions)
            .Include(p => p.StoplistEntries)
            .Include(p => p.ArrivalsDepartures)
            .Include(p => p.SecurityFindings)
            .Include(p => p.Links);
    }
}
