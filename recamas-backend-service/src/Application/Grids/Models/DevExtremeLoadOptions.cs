namespace RECAMAS.Application.Grids.Models
{
    /// <summary>
    /// Strongly-typed representation of the DevExtreme DataGrid LoadOptions payload. Deserialized from
    /// the raw <see cref="DataGridRequest.LoadOptions"/> and mapped onto a <see cref="DataGridRequest"/>.
    /// </summary>
    public class DevExtremeLoadOptions
    {
        /// <summary>Number of rows to skip (paging offset).</summary>
        public int? Skip { get; set; }

        /// <summary>Page size: maximum number of rows to take.</summary>
        public int? Take { get; set; }

        /// <summary>Whether the client requested the total row count.</summary>
        public bool? RequireTotalCount { get; set; }

        /// <summary>The requested sort descriptors.</summary>
        public List<DevExtremeSortDescriptor>? Sort { get; set; }

        /// <summary>The raw DevExtreme filter expression (array or object form).</summary>
        public object? Filter { get; set; }

        /// <summary>The free-text search value.</summary>
        public string? SearchValue { get; set; }

        /// <summary>The specific fields to search in.</summary>
        public List<string>? SearchExpr { get; set; }

        /// <summary>The search operation type (for example <c>contains</c>, <c>startswith</c>).</summary>
        public string? SearchOperation { get; set; }

        /// <summary>The requested grouping descriptors.</summary>
        public List<DevExtremeGroupDescriptor>? Group { get; set; }

        /// <summary>Summary aggregations to compute over the whole dataset.</summary>
        public List<DevExtremeSummaryDescriptor>? TotalSummary { get; set; }

        /// <summary>Summary aggregations to compute per group.</summary>
        public List<DevExtremeSummaryDescriptor>? GroupSummary { get; set; }
    }
}
