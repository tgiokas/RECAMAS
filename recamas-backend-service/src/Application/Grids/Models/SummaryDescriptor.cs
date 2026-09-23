namespace RECAMAS.Application.Grids.Models
{
    /// <summary>
    /// Describes a single summary aggregation applied to a grid query: the field to aggregate and the
    /// aggregation type.
    /// </summary>
    public sealed class SummaryDescriptor
    {
        /// <summary>The field name to summarize.</summary>
        public string Selector { get; set; } = string.Empty;

        /// <summary>The aggregation type: one of <c>sum</c>, <c>avg</c>, <c>min</c>, <c>max</c> or <c>count</c>.</summary>
        public string SummaryType { get; set; } = string.Empty;
    }
}
