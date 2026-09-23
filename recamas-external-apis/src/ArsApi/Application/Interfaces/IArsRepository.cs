using ArsApi.Domain.Entities;
using Shared.Domain.Enums;

namespace ArsApi.Application.Interfaces;

public interface IArsRepository
{
    Task<IEnumerable<ArsRecord>> SearchAsync(
        string? arc, string? name, string? surname,
        Nationality? nationality, string? passportNo,
        DateTime? dateOfBirth, string? mdFileNumber,
        CancellationToken ct = default);

    Task<ArsRecord> CreateAsync(ArsRecord record, CancellationToken ct = default);
    Task<bool> AnyAsync(CancellationToken ct = default);
    Task<IEnumerable<ArsRecord>> GetAllAsync(CancellationToken ct = default);
}
