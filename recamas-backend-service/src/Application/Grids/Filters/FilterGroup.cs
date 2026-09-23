namespace RECAMAS.Application.Grids.Filters
{
    /// <summary>
    /// Composite node in a filter expression tree that combines its child nodes with a single logical
    /// operator (<c>and</c> or <c>or</c>).
    /// </summary>
    public sealed class FilterGroup : FilterNode
    {
        /// <summary>The logical operator combining the children: <c>and</c> or <c>or</c>.</summary>
        public string Operator { get; set; } = string.Empty;

        /// <summary>The child nodes (conditions and/or nested groups) combined by this group.</summary>
        public List<FilterNode> Children { get; set; } = new List<FilterNode>();

        /// <summary>
        /// Initializes a new <see cref="FilterGroup"/> and sets <see cref="FilterNode.Type"/> to <c>group</c>.
        /// </summary>
        public FilterGroup()
        {
            Type = "group";
        }
    }
}
