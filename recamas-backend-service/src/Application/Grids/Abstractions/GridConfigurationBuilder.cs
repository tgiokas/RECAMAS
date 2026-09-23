using RECAMAS.Application.Grids.Models;

namespace RECAMAS.Application.Grids.Abstractions
{
    /// <summary>
    /// Fluent builder for assembling a <see cref="GridConfiguration{TProjection}"/> by declaring
    /// fields, a default sort order and a maximum page size, then producing an immutable
    /// configuration via <see cref="Build"/>.
    /// </summary>
    /// <typeparam name="TProjection">The projection (row/DTO) type that the grid queries and returns.</typeparam>
    public sealed class GridConfigurationBuilder<TProjection>
    {
        private readonly Dictionary<string, GridField> _fields = new();
        private readonly List<SortDescriptor> _defaultSort = new();
        private int _maxPageSize = GridConfiguration<TProjection>.GlobalMaxPageSizeCap;

        /// <summary>
        /// Declares a grid field exposed to the client.
        /// </summary>
        /// <param name="name">Public field name as exposed to and referenced by the client.</param>
        /// <param name="dataType">CLR type of the field, used when building filter, sort and summary expressions.</param>
        /// <param name="searchable">Whether the field participates in free-text search. Defaults to <see langword="false"/>.</param>
        /// <param name="sortable">Whether the field can be sorted on. Defaults to <see langword="true"/>.</param>
        /// <param name="filterable">Whether the field can be filtered on. Defaults to <see langword="true"/>.</param>
        /// <param name="sourceMember">Optional underlying projection member name when it differs from <paramref name="name"/>.</param>
        /// <returns>The same builder instance to allow fluent chaining.</returns>
        public GridConfigurationBuilder<TProjection> Field(
            string name,
            Type dataType,
            bool searchable = false,
            bool sortable = true,
            bool filterable = true,
            string? sourceMember = null)
        {
            _fields[name] = new GridField
            {
                Name = name,
                DataType = dataType,
                Searchable = searchable,
                Sortable = sortable,
                Filterable = filterable,
                SourceMember = sourceMember
            };
            return this;
        }

        /// <summary>
        /// Appends a field to the default sort order applied when the request specifies no sorting.
        /// Call multiple times to build a multi-level default sort.
        /// </summary>
        /// <param name="field">Public field name to sort by.</param>
        /// <param name="desc">Whether to sort descending. Defaults to <see langword="false"/> (ascending).</param>
        /// <returns>The same builder instance to allow fluent chaining.</returns>
        public GridConfigurationBuilder<TProjection> DefaultSortBy(string field, bool desc = false)
        {
            _defaultSort.Add(new SortDescriptor { Field = field, Desc = desc });
            return this;
        }

        /// <summary>
        /// Sets the maximum page size for the grid. Note the configured value is still clamped by
        /// <see cref="GridConfiguration{TProjection}.GlobalMaxPageSizeCap"/> at query time.
        /// </summary>
        /// <param name="size">The maximum number of rows a single page may return.</param>
        /// <returns>The same builder instance to allow fluent chaining.</returns>
        public GridConfigurationBuilder<TProjection> MaxPageSize(int size)
        {
            _maxPageSize = size;
            return this;
        }

        /// <summary>
        /// Produces an immutable <see cref="GridConfiguration{TProjection}"/> from the declared fields,
        /// default sort and page size.
        /// </summary>
        /// <param name="key">The unique key identifying the resulting grid configuration.</param>
        /// <returns>A built, read-only grid configuration.</returns>
        public BuiltGridConfiguration Build(string key) => new(key, _fields, _defaultSort, _maxPageSize);

        /// <summary>
        /// The concrete, immutable <see cref="GridConfiguration{TProjection}"/> produced by
        /// <see cref="GridConfigurationBuilder{TProjection}.Build"/>.
        /// </summary>
        public sealed class BuiltGridConfiguration : GridConfiguration<TProjection>
        {
            private readonly IReadOnlyDictionary<string, GridField> _fields;
            private readonly IReadOnlyList<SortDescriptor> _defaultSort;
            private readonly int _maxPageSize;

            /// <summary>
            /// Initializes a new <see cref="BuiltGridConfiguration"/> with the supplied settings.
            /// </summary>
            /// <param name="key">The unique grid key.</param>
            /// <param name="fields">The configured fields keyed by public field name.</param>
            /// <param name="defaultSort">The default sort order.</param>
            /// <param name="maxPageSize">The maximum page size.</param>
            public BuiltGridConfiguration(string key, IReadOnlyDictionary<string, GridField> fields, IReadOnlyList<SortDescriptor> defaultSort, int maxPageSize)
            {
                Key = key;
                _fields = fields;
                _defaultSort = defaultSort;
                _maxPageSize = maxPageSize;
            }

            /// <inheritdoc/>
            public override string Key { get; }

            /// <inheritdoc/>
            public override IReadOnlyDictionary<string, GridField> Fields => _fields;

            /// <inheritdoc/>
            public override IReadOnlyList<SortDescriptor> DefaultSort => _defaultSort;

            /// <inheritdoc/>
            public override int MaxPageSize => _maxPageSize;
        }
    }
}
