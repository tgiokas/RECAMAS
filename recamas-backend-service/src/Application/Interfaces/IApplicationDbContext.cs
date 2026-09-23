namespace RECAMAS.Application.Interfaces;

public interface IApplicationDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// Exposes an entity type as a queryable set, used by the generic Grids framework
    /// (see Application/Grids) to build ad-hoc projections without Application referencing EF Core.
    IQueryable<TEntity> Set<TEntity>() where TEntity : class;
}
