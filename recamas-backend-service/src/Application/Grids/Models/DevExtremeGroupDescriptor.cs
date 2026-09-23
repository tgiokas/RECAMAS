namespace RECAMAS.Application.Grids.Models
{
    /// <summary>
    /// Raw grouping descriptor as sent by the DevExtreme client inside LoadOptions, mapped onto a
    /// <see cref="GroupDescriptor"/> during request processing.
    /// </summary>
    public class DevExtremeGroupDescriptor
    {
        /// <summary>The field selector (name) to group by.</summary>
        public string? Selector { get; set; }

        /// <summary>Whether groups are sorted descending.</summary>
        public bool Desc { get; set; }

        /// <summary>Whether the group is expanded (its items returned) rather than collapsed. Defaults to <see langword="true"/>.</summary>
        public bool IsExpanded { get; set; } = true;

        /// <summary>Optional grouping interval for numeric/date grouping (for example <c>year</c>, <c>month</c>, <c>day</c>).</summary>
        public object? GroupInterval { get; set; }
    }
}
