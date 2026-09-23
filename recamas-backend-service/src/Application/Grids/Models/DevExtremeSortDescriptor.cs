namespace RECAMAS.Application.Grids.Models
{
    /// <summary>
    /// Raw sort descriptor as sent by the DevExtreme client inside LoadOptions, mapped onto a
    /// <see cref="SortDescriptor"/> during request processing.
    /// </summary>
    public class DevExtremeSortDescriptor
    {
        /// <summary>The field selector (name) to sort by.</summary>
        public string? Selector { get; set; }

        /// <summary>Alternative property name used as the sort field when <see cref="Selector"/> is not supplied.</summary>
        public string? Field { get; set; }

        /// <summary>Whether the sort is descending.</summary>
        public bool Desc { get; set; }
    }
}
