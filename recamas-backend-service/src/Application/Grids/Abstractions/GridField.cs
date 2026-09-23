namespace RECAMAS.Application.Grids.Abstractions
{
    /// <summary>
    /// Describes a single field exposed by a grid: its public name, CLR type and the
    /// search/sort/filter capabilities the framework permits for it when building queries.
    /// </summary>
    public sealed class GridField
    {
        /// <summary>Public field name exposed to and referenced by the client.</summary>
        public string Name { get; init; } = string.Empty;

        /// <summary>CLR type of the field, used when building filter, sort and summary expressions.</summary>
        public Type DataType { get; init; } = typeof(string);

        /// <summary>Whether this field participates in free-text search.</summary>
        public bool Searchable { get; init; } = false;

        /// <summary>Whether this field can be sorted on.</summary>
        public bool Sortable { get; init; } = true;

        /// <summary>Whether this field can be filtered on.</summary>
        public bool Filterable { get; init; } = true;

        /// <summary>
        /// Optional name of the underlying projection member when it differs from <see cref="Name"/>.
        /// When <see langword="null"/>, <see cref="Name"/> is used as the member name.
        /// </summary>
        public string? SourceMember { get; init; }
    }
}
