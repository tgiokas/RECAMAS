using CassApi.Domain.Entities;
using Shared.Domain.Enums;

namespace CassApi.Application.Interfaces;

public interface ICassRepository
{
    Task<IEnumerable<CassRecord>> SearchAsync(
        string? arc, string? name, string? surname, Nationality? nationality,
        string? passportNo, DateTime? dateOfBirth, string? cassFileNo,
        CancellationToken ct = default);
    Task<CassRecord> CreateAsync(CassRecord record, CancellationToken ct = default);
    Task<bool> AnyAsync(CancellationToken ct = default);
}
