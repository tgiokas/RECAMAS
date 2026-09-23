using RECAMAS.Application.Interfaces;

namespace RECAMAS.Application.Grids.Abstractions
{
    /// <summary>
    /// Strongly-typed source for a grid: supplies the base <see cref="IQueryable{T}"/> of projections
    /// to query and the <see cref="GridConfiguration{TProjection}"/> describing how it may be queried.
    /// </summary>
    /// <typeparam name="TProjection">The projection (row/DTO) type that the grid queries and returns.</typeparam>
    public interface IGridSourceProvider<TProjection>
    {
        /// <summary>
        /// Builds the base query of projections, using the supplied database context to access data sources.
        /// Filtering, search, sorting, grouping and paging are applied on top of this query by the framework.
        /// </summary>
        /// <param name="dbContext">The database context providing access to entity sets.</param>
        /// <returns>The base <see cref="IQueryable{T}"/> of projections.</returns>
        IQueryable<TProjection> Query(IApplicationDbContext dbContext);

        /// <summary>The configuration describing the fields, default sort and paging limits for this grid.</summary>
        GridConfiguration<TProjection> Configuration { get; }
    }
}
