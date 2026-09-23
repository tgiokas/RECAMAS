using StoplistApi.Domain.Entities;
using Shared.Domain.Enums;

namespace StoplistApi.Application.Interfaces;

public interface IStoplistRepository
{
    Task<IEnumerable<StoplistRecord>> SearchAsync(
        string? arc, string? name, string? surname, Nationality? nationality,
        string? passportNo, DateTime? dateOfBirth, CancellationToken ct = default);
    Task<StoplistRecord> CreateAsync(StoplistRecord record, CancellationToken ct = default);
    Task<bool> AnyAsync(CancellationToken ct = default);
}
