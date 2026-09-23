namespace RECAMAS.Application.Grids.Models;

/// <summary>
/// Result of a server-side grid query: the page of rows (or grouped items), counts and
/// summary aggregation results.
/// </summary>
/// <typeparam name="T">The element type of the returned data (a row projection or grouped item).</typeparam>
public sealed class DataGridResponse<T>
{
    /// <summary>The page of returned items (rows or grouped items).</summary>
    public IReadOnlyList<T> Data { get; init; } = Array.Empty<T>();

    /// <summary>Total number of rows matching the filter/search, before paging.</summary>
    public int TotalCount { get; init; }

    /// <summary>Top-level group count when grouping is applied, or <see langword="null"/> otherwise.</summary>
    public int? GroupCount { get; init; }

    /// <summary>Total summary aggregation results, or <see langword="null"/> when none were requested.</summary>
    public List<object>? Summary { get; init; }

    /// <summary>Optional provider-supplied metadata attached to the response.</summary>
    public object? Meta { get; init; }
}
