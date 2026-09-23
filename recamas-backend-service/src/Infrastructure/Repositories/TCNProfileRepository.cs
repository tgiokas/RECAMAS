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


    /// <summary>
    /// searches for potential duplicate TCN profiles based on the provided criteria.
    /// </summary>
    /// <param name="arc">The ARC (Alien Registration Card) number to search for.</param>
    /// <param name="passportNumber">The passport number to search for.</param>
    /// <param name="firstName">The first name to search for.</param>
    /// <param name="lastName">The last name to search for.</param>
    /// <param name="dateOfBirth">The date of birth to search for.</param>
    /// <param name="cancellationToken">A cancellation token for the async operation.</param>
    /// <returns>A list of TCN profiles that potentially match the provided criteria.</returns>
    /// <remarks>
    /// This method performs a search for potential duplicate TCN profiles by matching the provided ARC, passport number,
    /// and/or full name with date of birth. It uses trigram similarity for name matching to account for minor variations.
    /// All these data (data from ARS, CASS, Police, etc. and potential duplicates) must be forwarded to the frontend in order to allow user to select 
    /// the desired TCN profile to act with.
    /// </remarks>
    public async Task<IReadOnlyList<TcnProfile>> SearchForDuplicatesAsync(
        string? arc, string? passportNumber, string? firstName, string? lastName, DateOnly? dateOfBirth,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(arc) && string.IsNullOrWhiteSpace(passportNumber)
            && string.IsNullOrWhiteSpace(firstName) && string.IsNullOrWhiteSpace(lastName))
            return [];

        var normalizedArc = arc?.Trim().ToUpperInvariant();
        var normalizedPassport = passportNumber?.Trim().ToUpperInvariant();
        var hasNameMatchCriteria = !string.IsNullOrWhiteSpace(firstName)
            && !string.IsNullOrWhiteSpace(lastName)
            && dateOfBirth.HasValue;
        var query = _dbContext.TcnProfiles.AsNoTracking().Include(e => e.IdentityDocuments).AsQueryable();
        query = query.Where(e =>
            (normalizedArc != null && e.Arc != null && e.Arc.Trim().ToUpper() == normalizedArc) ||
            (normalizedPassport != null && e.IdentityDocuments.Any(d => d.DocumentNumber != null && d.DocumentNumber.Trim().ToUpper() == normalizedPassport)) ||
            (hasNameMatchCriteria
                && e.DateOfBirth == dateOfBirth
                && EF.Functions.TrigramsAreSimilar(e.FirstNameEn ?? string.Empty, firstName!)
                && EF.Functions.TrigramsAreSimilar(e.LastNameEn ?? string.Empty, lastName!)));
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
