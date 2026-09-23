using System.Text.Json.Serialization;

namespace RECAMAS.Application.Grids.Models
{
    /// <summary>
    /// A single group in a grouped grid response, in the shape DevExtreme expects: the group key,
    /// its child items (or nested groups), the item count and any group summaries.
    /// </summary>
    public sealed class GroupedDataItem
    {
        /// <summary>The group key value, kept strongly typed for correct client-side filtering.</summary>
        [JsonPropertyName("key")]
        public object? Key { get; set; }

        /// <summary>The group's items (rows or nested groups), or <see langword="null"/> when the group is not expanded.</summary>
        [JsonPropertyName("items")]
        public List<object>? Items { get; set; }

        /// <summary>The number of items in this group.</summary>
        [JsonPropertyName("count")]
        public int Count { get; set; }

        /// <summary>The summary aggregation results computed for this group, or <see langword="null"/> when none.</summary>
        [JsonPropertyName("summary")]
        public List<object>? Summary { get; set; }
    }
}
