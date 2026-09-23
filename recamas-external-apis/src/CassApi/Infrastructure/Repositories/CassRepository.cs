using CassApi.Application.Interfaces;
using CassApi.Domain.Entities;
using CassApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Enums;

namespace CassApi.Infrastructure.Repositories;

public class CassRepository(CassDbContext db) : ICassRepository
{
    public async Task<IEnumerable<CassRecord>> SearchAsync(
        string? arc, string? name, string? surname, Nationality? nationality,
        string? passportNo, DateTime? dateOfBirth, string? cassFileNo,
        CancellationToken ct = default)
    {
        var q = db.CassRecords.AsQueryable();
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
        if (!string.IsNullOrWhiteSpace(cassFileNo))
            q = q.Where(x => x.CassFileNo != null && x.CassFileNo.ToLower().Contains(cassFileNo.ToLower()));
        return await q.OrderBy(x => x.LastName).ToListAsync(ct);
    }

    public async Task<CassRecord> CreateAsync(CassRecord record, CancellationToken ct = default)
    {
        db.CassRecords.Add(record);
        await db.SaveChangesAsync(ct);
        return record;
    }

    public async Task<bool> AnyAsync(CancellationToken ct = default)
        => await db.CassRecords.AnyAsync(ct);
}
