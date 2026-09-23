using ArrivalsApi.Domain.Entities;
using Shared.Domain.Enums;

namespace ArrivalsApi.Application.Interfaces;

public interface IArrivalsRepository
{
    Task<IEnumerable<ArrivalRecord>> SearchAsync(
        string? arc, string? name, string? surname, Nationality? nationality,
        string? passportNo, DateTime? dateOfBirth, CancellationToken ct = default);
    Task<ArrivalRecord> CreateAsync(ArrivalRecord record, CancellationToken ct = default);
    Task<bool> AnyAsync(CancellationToken ct = default);
}
