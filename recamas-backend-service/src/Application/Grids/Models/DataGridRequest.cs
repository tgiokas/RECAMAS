using RECAMAS.Application.Grids.Filters;

namespace RECAMAS.Application.Grids.Models;

/// <summary>
/// Describes a single server-side grid query: paging, sorting, filtering, free-text search,
/// grouping, summaries and field projection. Either populated directly or derived from a
/// raw DevExtreme <see cref="DevExtremeLoadOptions"/> payload during query execution.
/// </summary>
public sealed class DataGridRequest
{
    /// <summary>
    /// Raw DevExtreme LoadOptions payload for direct pass-through. When present, it is deserialized
    /// and mapped onto the other properties of this request before the query runs.
    /// </summary>
    public object? LoadOptions { get; set; }

    /// <summary>Number of rows to skip (paging offset). Defaults to <c>0</c>.</summary>
    public int Skip { get; set; } = 0;

    /// <summary>Page size: maximum number of rows to take. Defaults to <c>20</c>.</summary>
    public int Take { get; set; } = 20;

    /// <summary>The requested sort order, or <see langword="null"/> to apply the grid's default sort.</summary>
    public List<SortDescriptor>? Sort { get; set; }

    /// <summary>The filter expression tree to apply, or <see langword="null"/> for no filtering.</summary>
    public FilterNode? Filter { get; set; }

    /// <summary>The free-text search value, or <see langword="null"/>/empty for no search.</summary>
    public string? SearchValue { get; set; }

    /// <summary>The specific fields to search in. When <see langword="null"/>, all searchable fields are used.</summary>
    public List<string>? SearchExpr { get; set; }

    /// <summary>The search operation type (for example <c>contains</c>, <c>startswith</c>, <c>endswith</c>, <c>eq</c>). Defaults to <c>contains</c>.</summary>
    public string? SearchOperation { get; set; } = "contains";

    /// <summary>The grouping descriptors to apply, or <see langword="null"/> for ungrouped results.</summary>
    public List<GroupDescriptor>? Group { get; set; }

    /// <summary>The summary aggregations to compute per group, or <see langword="null"/> for none.</summary>
    public List<SummaryDescriptor>? GroupSummary { get; set; }

    /// <summary>Whether the total row count should be computed and returned. Defaults to <see langword="true"/>.</summary>
    public bool RequireTotalCount { get; set; } = true;

    /// <summary>Whether the top-level group count should be computed and returned. Defaults to <see langword="false"/>.</summary>
    public bool RequireGroupCount { get; set; } = false;

    /// <summary>The summary aggregations to compute over the whole filtered dataset, or <see langword="null"/> for none.</summary>
    public List<SummaryDescriptor>? TotalSummary { get; set; }

    /// <summary>Field projection: the subset of fields to select, or <see langword="null"/> to return all.</summary>
    public List<string>? Select { get; set; }

    /// <summary>Arbitrary caller-supplied data passed through with the request.</summary>
    public object? UserData { get; set; }
}
