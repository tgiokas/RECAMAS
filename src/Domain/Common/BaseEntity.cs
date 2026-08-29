namespace RECAMAS.Domain.Common;

/// Base type for every entity across every module/schema.
///
/// Key conventions (locked in during architecture design):
///  - Internal primary key is a bigint identity (Id) — cheap joins, small indexes.
///  - PublicId (Guid) is only populated for entities exposed outside the service
///    (e.g. across an HTTP boundary to the SPA or another microservice). Not every
///    entity needs one — add it only when the entity is actually referenced externally.
///  - Soft delete everywhere: IsDeleted flag, never a hard DELETE. Combined with
///    global EF Core query filters (see ApplicationDbContext) so deleted rows are
///    invisible by default without repositories having to remember to filter them.
///  - Audit columns are populated by a SaveChanges interceptor in Infrastructure,
///    not by callers — services should never set these manually.
public abstract class BaseEntity
{
    public long Id { get; set; }

    /// Populate only on entities that cross a service/HTTP boundary
    /// (e.g. Case.PublicId, TCNProfile.PublicId). Internal-only entities
    /// (e.g. a join/history table nobody references externally) can leave this null.
    public Guid? PublicId { get; set; }

    public bool IsDeleted { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public string? CreatedBy { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }

    /// EF Core concurrency token (Postgres "xmin" mapping is wired in
    /// ApplicationDbContext) — prevents silent overwrite when two officers
    /// edit the same case/rule at once.
    public uint RowVersion { get; set; }
}
