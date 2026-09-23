namespace RECAMAS.Application.Grids.Models
{
    /// <summary>
    /// Raw summary descriptor as sent by the DevExtreme client inside LoadOptions, mapped onto a
    /// <see cref="SummaryDescriptor"/> during request processing.
    /// </summary>
    public class DevExtremeSummaryDescriptor
    {
        /// <summary>The field selector (name) to summarize.</summary>
        public string? Selector { get; set; }

        /// <summary>The summary aggregation type (for example <c>sum</c>, <c>avg</c>, <c>min</c>, <c>max</c>, <c>count</c>).</summary>
        public string? SummaryType { get; set; }
    }
}
