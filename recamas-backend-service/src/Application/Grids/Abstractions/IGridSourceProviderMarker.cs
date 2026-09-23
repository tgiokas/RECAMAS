using RECAMAS.Application.Grids.Models;
using RECAMAS.Application.Interfaces;

namespace RECAMAS.Application.Grids.Abstractions
{
    /// <summary>
    /// Non-generic marker abstraction over a grid source provider, allowing providers of differing
    /// projection types to be registered and resolved in a single list and executed without knowing
    /// their generic projection type.
    /// </summary>
    public interface IGridSourceProviderMarker
    {
        /// <summary>The unique key identifying the grid this provider serves.</summary>
        string Key { get; }

        /// <summary>
        /// The normalized permission required to query this grid, or <see langword="null"/> when no
        /// permission check beyond authentication is required.
        /// </summary>
        string? RequiredPermission { get; }

        /// <summary>
        /// Executes the grid query: applies LoadOptions mapping, filtering, search, sorting,
        /// grouping, paging and summaries, then returns the resulting page.
        /// </summary>
        /// <param name="dbContext">The database context providing access to entity sets.</param>
        /// <param name="request">The request describing paging, sorting, filtering, search, grouping and summaries.</param>
        /// <param name="ct">A token to observe for cancellation.</param>
        /// <returns>A task producing the <see cref="DataGridResponse{T}"/> of <see cref="object"/> rows or grouped items.</returns>
        Task<DataGridResponse<object>> ExecuteAsync(IApplicationDbContext dbContext, DataGridRequest request, CancellationToken ct);
    }
}
