namespace RECAMAS.Application.Grids.Models
{
    /// <summary>
    /// Describes one level of grouping applied to a grid query: the field to group by, sort
    /// direction, expansion state and optional numeric/date interval.
    /// </summary>
    public sealed class GroupDescriptor
    {
        /// <summary>The field name to group by.</summary>
        public string Selector { get; set; } = string.Empty;

        /// <summary>Whether groups are sorted in descending order. Defaults to <see langword="false"/>.</summary>
        public bool Desc { get; set; } = false;

        /// <summary>
        /// Whether the group is expanded. When <see langword="true"/> the group's items are returned;
        /// when <see langword="false"/> the items are returned as <see langword="null"/>. Defaults to <see langword="true"/>.
        /// </summary>
        public bool IsExpanded { get; set; } = true;

        /// <summary>Optional grouping interval for numeric/date grouping (for example <c>year</c>, <c>month</c>, <c>day</c>).</summary>
        public object? GroupInterval { get; set; }
    }
}
