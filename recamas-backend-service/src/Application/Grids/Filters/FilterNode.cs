namespace RECAMAS.Application.Grids.Filters
{
    /// <summary>
    /// Base type for a node in a grid filter expression tree. A node is either a leaf
    /// <see cref="FilterCondition"/> or a logical <see cref="FilterGroup"/> of child nodes.
    /// </summary>
    public abstract class FilterNode
    {
        /// <summary>Discriminator identifying the concrete node kind: <c>group</c> or <c>condition</c>.</summary>
        public string Type { get; set; } = string.Empty;
    }
}
