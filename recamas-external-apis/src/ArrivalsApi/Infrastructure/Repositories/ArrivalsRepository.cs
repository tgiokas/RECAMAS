using ArrivalsApi.Application.Interfaces;
using ArrivalsApi.Domain.Entities;
using ArrivalsApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Enums;

namespace ArrivalsApi.Infrastructure.Repositories;

public class ArrivalsRepository(ArrivalsDbContext db) : IArrivalsRepository
{
    public async Task<IEnumerable<ArrivalRecord>> SearchAsync(
        string? arc, string? name, string? surname, Nationality? nationality,
        string? passportNo, DateTime? dateOfBirth, CancellationToken ct = default)
    {
        var q = db.ArrivalRecords.AsQueryable();
        if (!string.IsNullOrWhiteSpace(arc))
            q = q.Where(x => x.Arc.ToLower().Contains(arc.ToLower()));
        if (!string.IsNullOrWhiteSpace(name))
            q = q.Where(x => x.FirstName.ToLower().Contains(name.ToLower()));
        if (!string.IsNullOrWhiteSpace(surname))
            q = q.Where(x => x.LastName.ToLower().Contains(surname.ToLower()));
        if (nationality.HasValue)
            q = q.Where(x => x.Nationality == nationality.Value);
        if (!string.IsNullOrWhiteSpace(passportNo))
            q = q.Where(x => x.PassportNo != null && x.PassportNo.ToLower().Contains(passportNo.ToLower()));
        if (dateOfBirth.HasValue)
            q = q.Where(x => x.DateOfBirth.Date == dateOfBirth.Value.Date);
        return await q.OrderBy(x => x.Arc).ThenByDescending(x => x.MovementDate).ToListAsync(ct);
    }

    public async Task<ArrivalRecord> CreateAsync(ArrivalRecord record, CancellationToken ct = default)
    {
        db.ArrivalRecords.Add(record);
        await db.SaveChangesAsync(ct);
        return record;
    }

    public async Task<bool> AnyAsync(CancellationToken ct = default)
        => await db.ArrivalRecords.AnyAsync(ct);
}
