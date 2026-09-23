using ArsApi.Application.Interfaces;
using ArsApi.Domain.Entities;
using ArsApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Enums;

namespace ArsApi.Infrastructure.Repositories;

public class ArsRepository(ArsDbContext db) : IArsRepository
{
    public async Task<IEnumerable<ArsRecord>> SearchAsync(
        string? arc, string? name, string? surname, Nationality? nationality,
        string? passportNo, DateTime? dateOfBirth, string? mdFileNumber,
        CancellationToken ct = default)
    {
        var q = db.ArsRecords.AsQueryable();

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
        if (!string.IsNullOrWhiteSpace(mdFileNumber))
            q = q.Where(x => x.MdFileNo != null && x.MdFileNo.ToLower().Contains(mdFileNumber.ToLower()));

        return await q.OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ToListAsync(ct);
    }

    public async Task<ArsRecord> CreateAsync(ArsRecord record, CancellationToken ct = default)
    {
        db.ArsRecords.Add(record);
        await db.SaveChangesAsync(ct);
        return record;
    }

    public async Task<bool> AnyAsync(CancellationToken ct = default)
        => await db.ArsRecords.AnyAsync(ct);

    public async Task<IEnumerable<ArsRecord>> GetAllAsync(CancellationToken ct = default)
        => await db.ArsRecords.OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ToListAsync(ct);
}
