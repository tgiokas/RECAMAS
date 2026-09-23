namespace RECAMAS.Application.Grids.Models
{
    /// <summary>
    /// Describes a single sort level applied to a grid query: the field and its direction.
    /// </summary>
    public sealed class SortDescriptor
    {
        /// <summary>The field name to sort by.</summary>
        public string Field { get; set; } = string.Empty;

        /// <summary>Whether the sort is descending. Defaults to <see langword="false"/> (ascending).</summary>
        public bool Desc { get; set; } = false;
    }
}
