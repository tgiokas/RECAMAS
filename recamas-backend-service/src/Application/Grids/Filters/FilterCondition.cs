namespace RECAMAS.Application.Grids.Filters
{
    /// <summary>
    /// Leaf node in a filter expression tree representing a single comparison of a field against a value
    /// (for example <c>field eq value</c> or <c>field contains value</c>).
    /// </summary>
    public sealed class FilterCondition : FilterNode
    {
        /// <summary>The field name this condition applies to.</summary>
        public string Field { get; set; } = string.Empty;

        /// <summary>
        /// The normalized comparison operator (for example <c>eq</c>, <c>neq</c>, <c>gt</c>, <c>contains</c>,
        /// <c>startswith</c>, <c>isnull</c>).
        /// </summary>
        public string Operator { get; set; } = string.Empty;

        /// <summary>The comparison value, or <see langword="null"/> for operators that take no value.</summary>
        public object? Value { get; set; }

        /// <summary>
        /// Initializes a new <see cref="FilterCondition"/> and sets <see cref="FilterNode.Type"/> to <c>condition</c>.
        /// </summary>
        public FilterCondition()
        {
            Type = "condition";
        }
    }
}
