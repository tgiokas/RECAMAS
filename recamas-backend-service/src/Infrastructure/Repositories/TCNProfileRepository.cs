using Microsoft.EntityFrameworkCore;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Interfaces;
using RECAMAS.Infrastructure.Database;

namespace RECAMAS.Infrastructure.Repositories;

public sealed class TcnProfileRepository : ITcnProfileRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TcnProfileRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

    public Task<TcnProfile?> GetByIdWithDetailsAsync(long id, CancellationToken cancellationToken = default) =>
        FullGraph(_dbContext.TcnProfiles.AsNoTracking()).FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    public Task<TcnProfile?> GetByPublicIdWithDetailsAsync(Guid publicId, CancellationToken cancellationToken = default) =>
        FullGraph(_dbContext.TcnProfiles.AsNoTracking()).FirstOrDefaultAsync(e => e.PublicId == publicId, cancellationToken);

    public Task<TcnProfile?> GetByIdForUpdateAsync(long id, CancellationToken cancellationToken = default) =>
        FullGraph(_dbContext.TcnProfiles).FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    public Task<TcnProfile?> GetByArcAsync(string arc, CancellationToken cancellationToken = default) =>
        _dbContext.TcnProfiles.AsNoTracking().FirstOrDefaultAsync(e => e.Arc == arc, cancellationToken);

    public async Task<IReadOnlyList<TcnProfile>> SearchForDuplicatesAsync(
        string? arc, string? passportNumber, string? firstName, string? lastName,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(arc) && string.IsNullOrWhiteSpace(passportNumber)
            && string.IsNullOrWhiteSpace(firstName) && string.IsNullOrWhiteSpace(lastName))
            return [];

        var query = _dbContext.TcnProfiles.AsNoTracking().Include(e => e.IdentityDocuments).AsQueryable();
        query = query.Where(e =>
            (!string.IsNullOrWhiteSpace(arc) && e.Arc == arc) ||
            (!string.IsNullOrWhiteSpace(passportNumber) && e.IdentityDocuments.Any(d => d.DocumentNumber == passportNumber)) ||
            (!string.IsNullOrWhiteSpace(firstName) && EF.Functions.TrigramsAreSimilar(e.FirstNameEn ?? string.Empty, firstName)) ||
            (!string.IsNullOrWhiteSpace(lastName) && EF.Functions.TrigramsAreSimilar(e.LastNameEn ?? string.Empty, lastName)));
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<(IReadOnlyList<TcnProfile> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, string? quickSearchTerm, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.TcnProfiles.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(quickSearchTerm))
            query = query.Where(e => e.Arc == quickSearchTerm || e.RecamasId == quickSearchTerm ||
                EF.Functions.TrigramsAreSimilar(e.FirstNameEn ?? string.Empty, quickSearchTerm) ||
                EF.Functions.TrigramsAreSimilar(e.LastNameEn ?? string.Empty, quickSearchTerm));

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query.OrderByDescending(e => e.CreatedAt)
            .Skip(Math.Max(0, page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        return (items, totalCount);
    }

    public async Task AddAsync(TcnProfile profile, CancellationToken cancellationToken = default)
    {
        if (profile.PublicId == Guid.Empty)
            profile.PublicId = Guid.NewGuid();

        await _dbContext.TcnProfiles.AddAsync(profile, cancellationToken);
    }

    private static IQueryable<TcnProfile> FullGraph(IQueryable<TcnProfile> query) => query.AsSplitQuery()
        .Include(e => e.Nationalities)
        .Include(e => e.IdentityDocuments)
        .Include(e => e.ResidencyStatuses)
        .Include(e => e.ResidencyApplications)
        .Include(e => e.IpStatuses)
        .Include(e => e.IpApplications)
        .Include(e => e.Appeals)
        .Include(e => e.ReturnDecisions)
        .Include(e => e.StoplistEntries)
        .Include(e => e.ArrivalDepartures)
        .Include(e => e.SecurityDetails).ThenInclude(e => e.Findings)
        .Include(e => e.LinkedProfilesFrom)
        .Include(e => e.LinkedProfilesTo);
}
