using RECAMAS.Domain.Entities.TCNProfile;

namespace RECAMAS.Domain.Interfaces;

public interface ITcnProfileRepository
{
    Task<TcnProfile?> GetByIdWithDetailsAsync(long id, CancellationToken cancellationToken = default);
    Task<TcnProfile?> GetByPublicIdWithDetailsAsync(Guid publicId, CancellationToken cancellationToken = default);
    Task<TcnProfile?> GetByIdForUpdateAsync(long id, CancellationToken cancellationToken = default);
    Task<TcnProfile?> GetByArcAsync(string arc, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TcnProfile>> SearchForDuplicatesAsync(
        string? arc,
        string? passportNumber,
        string? firstName,
        string? lastName,
        CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<TcnProfile> Items, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        string? quickSearchTerm,
        CancellationToken cancellationToken = default);
    Task AddAsync(TcnProfile profile, CancellationToken cancellationToken = default);
}
