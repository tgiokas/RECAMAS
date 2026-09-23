using RECAMAS.Application.Grids.Models;

namespace RECAMAS.Application.Grids.Abstractions;

/// <summary>
/// Entry point for executing server-side grid queries. Resolves the grid source provider
/// registered for a given grid key and dispatches the request to it.
/// </summary>
public interface IGridQueryService
{
    /// <summary>
    /// Executes a grid query against the provider registered for the supplied key.
    /// </summary>
    /// <param name="gridKey">The unique key identifying the target grid.</param>
    /// <param name="request">The request describing paging, sorting, filtering, search, grouping and summaries.</param>
    /// <param name="ct">A token to observe for cancellation.</param>
    /// <returns>
    /// A task producing a <see cref="DataGridResponse{T}"/> of <see cref="object"/> rows
    /// (or grouped items) along with total/group counts and summary results.
    /// </returns>
    /// <exception cref="InvalidOperationException">Thrown when no provider is registered for <paramref name="gridKey"/>.</exception>
    Task<DataGridResponse<object>> QueryAsync(string gridKey, DataGridRequest request, CancellationToken ct = default);

    /// <summary>
    /// Returns the grid source provider registered for the given key, or <see langword="null"/> if none is found.
    /// Unlike <see cref="QueryAsync"/>, this does not throw when the key is unknown.
    /// </summary>
    /// <param name="gridKey">The unique key identifying the target grid.</param>
    /// <returns>The matching <see cref="IGridSourceProviderMarker"/>, or <see langword="null"/> if not found.</returns>
    IGridSourceProviderMarker? GetProvider(string gridKey);
}
