using RECAMAS.Application.Grids.Models;

namespace RECAMAS.Application.Grids.Abstractions;

/// <summary>
/// Base class describing how a server-side data grid is queried: the set of exposed
/// <see cref="GridField"/> definitions, the default sort order and paging limits.
/// Concrete grids derive from this type (directly or via <see cref="GridConfigurationBuilder{TProjection}"/>)
/// to declare which projected members can be searched, sorted, filtered and paged.
/// </summary>
/// <typeparam name="TProjection">The projection (row/DTO) type that the grid queries and returns.</typeparam>
public abstract class GridConfiguration<TProjection>
{
    /// <summary>
    /// Absolute upper bound on the page size that any grid may serve, regardless of the
    /// per-grid <see cref="MaxPageSize"/>. Acts as a hard safety cap to prevent unbounded result sets.
    /// </summary>
    public const int GlobalMaxPageSizeCap = 1000;

    /// <summary>
    /// Unique key identifying this grid configuration. Used to resolve the matching
    /// grid source provider when a query is dispatched by key.
    /// </summary>
    public abstract string Key { get; }

    /// <summary>
    /// The fields exposed by this grid, keyed by the public field name used by the client.
    /// Each entry describes the field's CLR type and its search/sort/filter capabilities.
    /// </summary>
    public abstract IReadOnlyDictionary<string, GridField> Fields { get; }

    /// <summary>
    /// The sort order applied when the incoming request does not specify any sorting.
    /// Defaults to an empty list (no default sort).
    /// </summary>
    public virtual IReadOnlyList<SortDescriptor> DefaultSort => Array.Empty<SortDescriptor>();

    /// <summary>
    /// The maximum page size this grid will serve. Defaults to <see cref="GlobalMaxPageSizeCap"/>
    /// and is itself further clamped by that global cap.
    /// </summary>
    public virtual int MaxPageSize => GlobalMaxPageSizeCap;

    /// <summary>
    /// Attempts to look up a configured field by its public name.
    /// </summary>
    /// <param name="field">The public field name to resolve.</param>
    /// <param name="gridField">When this method returns <see langword="true"/>, contains the matching <see cref="GridField"/>; otherwise the default value.</param>
    /// <returns><see langword="true"/> if a field with the given name exists; otherwise <see langword="false"/>.</returns>
    public bool TryGetField(string field, out GridField gridField)
        => Fields.TryGetValue(field, out gridField!);
}
