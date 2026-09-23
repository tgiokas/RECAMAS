using StoplistApi.Application.Interfaces;
using StoplistApi.Domain.Entities;
using StoplistApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Enums;

namespace StoplistApi.Infrastructure.Repositories;

public class StoplistRepository(StoplistDbContext db) : IStoplistRepository
{
    public async Task<IEnumerable<StoplistRecord>> SearchAsync(
        string? arc, string? name, string? surname, Nationality? nationality,
        string? passportNo, DateTime? dateOfBirth, CancellationToken ct = default)
    {
        var q = db.StoplistRecords.AsQueryable();
        if (!string.IsNullOrWhiteSpace(arc))
            q = q.Where(x => x.Arc.ToLower().Contains(arc.ToLower()));
        if (!string.IsNullOrWhiteSpace(name))
            q = q.Where(x => x.FirstName.ToLower().Contains(name.ToLower()));
        if (!string.IsNullOrWhiteSpace(surname))
            q = q.Where(x => x.LastName.ToLower().Contains(surname.ToLower()));
        if (nationality.HasValue)
            q = q.Where(x => x.Nationality == nationality.Value);
        if (!string.IsNullOrWhiteSpace(passportNo))
            q = q.Where(x => x.PassportNo != null &&
                              x.PassportNo.ToLower().Contains(passportNo.ToLower()));
        if (dateOfBirth.HasValue)
            q = q.Where(x => x.DateOfBirth.Date == dateOfBirth.Value.Date);
        return await q.OrderBy(x => x.LastName).ToListAsync(ct);
    }

    public async Task<StoplistRecord> CreateAsync(StoplistRecord record,
        CancellationToken ct = default)
    {
        db.StoplistRecords.Add(record);
        await db.SaveChangesAsync(ct);
        return record;
    }

    public async Task<bool> AnyAsync(CancellationToken ct = default)
        => await db.StoplistRecords.AnyAsync(ct);
}
