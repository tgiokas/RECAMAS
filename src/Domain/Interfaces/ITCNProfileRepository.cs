using RECAMAS.Domain.Entities.TCNProfile;

namespace RECAMAS.Domain.Interfaces;

/// <summary>
/// One repository for the whole TCNProfile aggregate — the root plus its 12
/// child collections are loaded/saved together, never separately, matching
/// the CivilianPortal convention (IApplicationRepository, ICitizenUserRepository):
/// one purpose-built interface per aggregate root, no generic IRepository&lt;T&gt;.
/// Implemented in Infrastructure/Repositories/TCNProfileRepository.cs.
/// </summary>
public interface ITCNProfileRepository
{
    /// <summary>Full graph, read-only — for the "Profile Details" screen (Study 3.2.2), all 3 tabs at once.</summary>
    Task<TCNProfile?> GetByIdWithDetailsAsync(long id, CancellationToken ct = default);

    /// <summary>Same as <see cref="GetByIdWithDetailsAsync"/> but by the API-facing PublicId.</summary>
    Task<TCNProfile?> GetByPublicIdWithDetailsAsync(Guid publicId, CancellationToken ct = default);

    /// <summary>
    /// Full graph, tracked — for edit flows (Study 3.2.2 "Manual Profile Updates").
    /// Loaded whole rather than per-tab because the aggregate boundary is the
    /// whole profile: every child collection commits together in one SaveChanges.
    /// </summary>
    Task<TCNProfile?> GetByIdForUpdateAsync(long id, CancellationToken ct = default);

    /// <summary>
    /// Lightweight, no includes — used by the automatic interface-refresh
    /// triggers (Study 9.2.1/9.3.1/9.4.1/9.5.1: "Only for TCN Profiles where
    /// ARC is available") and basic existence checks, not for display.
    /// </summary>
    Task<TCNProfile?> GetByArcAsync(string arc, CancellationToken ct = default);

    /// <summary>
    /// Study 2.2.2 (Duplicate Detection): "raises an alert when key fields
    /// match across profiles/sources (e.g. ARC, passport number, name &amp;
    /// surname)". ARC and passport number are exact matches; first/last name
    /// use the GIN trigram index (TCNProfileConfiguration) via
    /// EF.Functions.TrigramsAreSimilar so near-matches surface too, per the
    /// Study's own "a name mismatch shall not preclude identifying related
    /// records" requirement (12.3.1).
    /// </summary>
    Task<IReadOnlyList<TCNProfile>> SearchForDuplicatesAsync(
        string? arc, string? passportNumber, string? firstName, string? lastName, CancellationToken ct = default);

    /// <summary>
    /// Study 3.2.1 (Profiles list) "Quick search" only — column-chooser/
    /// advanced-search filtering is a screen-specific read model for later,
    /// not part of this repository.
    /// </summary>
    Task<(IReadOnlyList<TCNProfile> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, string? quickSearchTerm, CancellationToken ct = default);

    /// <summary>No SaveChanges — caller commits the transaction (e.g. alongside an outbox message).</summary>
    Task AddWithoutSaveAsync(TCNProfile profile, CancellationToken ct = default);

    /// <summary>
    /// Caller already holds a tracked entity from <see cref="GetByIdForUpdateAsync"/>
    /// and has mutated it; this just commits. Audit columns (UpdatedAt/UpdatedBy)
    /// are set by a SaveChanges interceptor, not here — see BaseEntity remarks.
    /// </summary>
    Task UpdateAsync(TCNProfile profile, CancellationToken ct = default);
}
