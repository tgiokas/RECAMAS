using System.Text.Json;
using System.Linq.Expressions;
using System.Globalization;
using RECAMAS.Application.Grids.Abstractions;
using RECAMAS.Application.Grids.Filters;
using RECAMAS.Application.Grids.Helpers;
using RECAMAS.Application.Grids.Models;
using RECAMAS.Application.Interfaces;

namespace RECAMAS.Application.Grids.Services
{
    /// <summary>
    /// Default grid source provider that drives the full server-side query pipeline for a projection type:
    /// optional DevExtreme LoadOptions mapping, filtering, search, sorting, paging, single- and
    /// multi-level grouping, and summary aggregation. Concrete grids either use this type directly or
    /// derive from it to override the protected extension points
    /// (<see cref="ApplyRequestQueryTransforms"/>, <see cref="HydratePage"/>,
    /// <see cref="BuildResponseMetadata"/>, <see cref="RequiredPermission"/>).
    /// </summary>
    /// <typeparam name="TProjection">The projection (row/DTO) type that the grid queries and returns.</typeparam>
    public class GridSourceProvider<TProjection> : IGridSourceProvider<TProjection>, IGridSourceProviderMarker
    {
        private readonly Func<IApplicationDbContext, IQueryable<TProjection>> _queryFactory;

        /// <inheritdoc/>
        public GridConfiguration<TProjection> Configuration { get; }

        /// <inheritdoc/>
        public string Key => Configuration.Key;

        /// <summary>
        /// The permission required to query this grid. The default of <see langword="null"/> means no
        /// permission check beyond authentication. Production grid providers should override this with
        /// their normalized lowercase object name.
        /// </summary>
        public virtual string? RequiredPermission => null;

        /// <summary>
        /// Initializes a new <see cref="GridSourceProvider{TProjection}"/>.
        /// </summary>
        /// <param name="configuration">The configuration describing the grid's fields, default sort and paging limits.</param>
        /// <param name="queryFactory">Factory that builds the base projection query from a database context.</param>
        public GridSourceProvider(GridConfiguration<TProjection> configuration, Func<IApplicationDbContext, IQueryable<TProjection>> queryFactory)
        {
            Configuration = configuration;
            _queryFactory = queryFactory;
        }

        /// <inheritdoc/>
        public IQueryable<TProjection> Query(IApplicationDbContext dbContext) => _queryFactory(dbContext);

        /// <summary>
        /// Extension point invoked after the base query is built but before filtering, search, sorting
        /// and paging are applied, allowing derived providers to inject additional query transforms
        /// (for example request-scoped scoping). The default implementation returns the query unchanged.
        /// </summary>
        /// <param name="query">The base query to transform.</param>
        /// <param name="dbContext">The database context providing data access.</param>
        /// <param name="request">The current grid request.</param>
        /// <returns>The (possibly transformed) query.</returns>
        protected virtual IQueryable<TProjection> ApplyRequestQueryTransforms(
            IQueryable<TProjection> query,
            IApplicationDbContext dbContext,
            DataGridRequest request)
        {
            return query;
        }

        private void MapLoadOptionsToRequest(DataGridRequest request)
        {
            if (request.LoadOptions == null) return;

            try
            {
                // Deserialize the LoadOptions JSON to a dynamic object
                var loadOptionsJson = JsonSerializer.Serialize(request.LoadOptions);
                var loadOptions = JsonSerializer.Deserialize<DevExtremeLoadOptions>(loadOptionsJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (loadOptions == null) return;

                // Map DevExtreme LoadOptions to DataGridRequest properties
                request.Skip = loadOptions.Skip ?? request.Skip;
                request.Take = loadOptions.Take ?? request.Take;
                request.RequireTotalCount = loadOptions.RequireTotalCount ?? request.RequireTotalCount;

                // Map sorting
                if (loadOptions.Sort != null && loadOptions.Sort.Any())
                {
                    // Translate frontend selector names to backend-configured field keys
                    request.Sort = loadOptions.Sort.Select(s => new SortDescriptor
                    {
                        Field = TranslateSelector(s.Selector ?? s.Field ?? string.Empty),
                        Desc = s.Desc
                    }).ToList();
                }

                // Map filtering - convert DevExtreme filter format to our FilterNode format
                if (loadOptions.Filter != null)
                {
                    request.Filter = ConvertDevExtremeFilter(loadOptions.Filter);
                    // Normalize any field names inside the parsed filter to match our GridConfiguration
                    if (request.Filter != null)
                    {
                        NormalizeFilterFields(request.Filter);
                    }
                }

                // Map search
                request.SearchValue = loadOptions.SearchValue;
                // Translate any search expressions through the configuration
                if (loadOptions.SearchExpr != null && loadOptions.SearchExpr.Any())
                {
                    request.SearchExpr = loadOptions.SearchExpr.Select(s => TranslateSelector(s)).Where(s => !string.IsNullOrEmpty(s)).ToList();
                }
                else
                {
                    request.SearchExpr = loadOptions.SearchExpr;
                }
                request.SearchOperation = loadOptions.SearchOperation;

                // Map grouping
                if (loadOptions.Group != null && loadOptions.Group.Any())
                {
                    request.Group = loadOptions.Group.Select(g => new GroupDescriptor
                    {
                        Selector = g.Selector ?? string.Empty,
                        Desc = g.Desc,
                        IsExpanded = g.IsExpanded,
                        GroupInterval = g.GroupInterval
                    }).ToList();
                }

                // Map summaries
                if (loadOptions.TotalSummary != null && loadOptions.TotalSummary.Any())
                {
                    request.TotalSummary = loadOptions.TotalSummary.Select(s => new SummaryDescriptor
                    {
                        Selector = s.Selector ?? string.Empty,
                        SummaryType = s.SummaryType ?? string.Empty
                    }).ToList();
                }

                if (loadOptions.GroupSummary != null && loadOptions.GroupSummary.Any())
                {
                    request.GroupSummary = loadOptions.GroupSummary.Select(s => new SummaryDescriptor
                    {
                        Selector = s.Selector ?? string.Empty,
                        SummaryType = s.SummaryType ?? string.Empty
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                // Log error but continue with original request properties
                Console.WriteLine($"Error mapping LoadOptions: {ex.Message}");
            }
        }

        // Translate an incoming selector (from DevExtreme) to a valid grid field key from Configuration.
        private string TranslateSelector(string? selector)
        {
            if (string.IsNullOrWhiteSpace(selector)) return string.Empty;
            var config = Configuration;

            // Direct match
            if (config.TryGetField(selector, out _)) return selector!;

            // Case-insensitive key match
            var keyMatch = config.Fields.Keys.FirstOrDefault(k => string.Equals(k, selector, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(keyMatch)) return keyMatch;

            // Try to match by SourceMember (the underlying projection member)
            var sourceMatch = config.Fields.FirstOrDefault(kv => !string.IsNullOrEmpty(kv.Value.SourceMember) && string.Equals(kv.Value.SourceMember, selector, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(sourceMatch.Key)) return sourceMatch.Key;

            // As a last resort, try to match by SourceMember case-insensitively when selector might be camelCase/pascalCase
            sourceMatch = config.Fields.FirstOrDefault(kv => !string.IsNullOrEmpty(kv.Value.SourceMember) && string.Equals(kv.Value.SourceMember, selector, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(sourceMatch.Key)) return sourceMatch.Key;

            // No mapping found — return the selector as-is. GridExpressionBuilder will ignore unknown fields.
            return selector!;
        }

        // Recursively translate fields inside a FilterNode tree to configured field keys
        private void NormalizeFilterFields(FilterNode node)
        {
            if (node == null) return;
            switch (node)
            {
                case FilterCondition filterCondition:
                    if (!string.IsNullOrEmpty(filterCondition.Field))
                    {
                        filterCondition.Field = TranslateSelector(filterCondition.Field);
                    }
                    break;
                case FilterGroup filterGroup:
                    foreach (var child in filterGroup.Children)
                    {
                        NormalizeFilterFields(child);
                    }
                    break;
            }
        }

        private FilterNode? ConvertDevExtremeFilter(object devExtremeFilter)
        {
            if (devExtremeFilter == null) return null;

            if (devExtremeFilter is JsonElement jsonElement)
            {
                if (jsonElement.GetRawText().Contains("NaN")) return null;
            }

            try
            {
                if (devExtremeFilter is JsonElement je && je.ValueKind == JsonValueKind.Array)
                    return ParseFilterArray(je);

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error converting filter: {ex.Message}");
                return null;
            }
        }

        private FilterNode? ParseFilterArray(JsonElement je)
        {
            var array = je.EnumerateArray().ToArray();
            if (array.Length == 0) return null;

            // Negation: ["!", condition] — invert parsed child filter.
            if (array.Length == 2
                && array[0].ValueKind == JsonValueKind.String
                && array[0].GetString() == "!")
            {
                var child = ParseFilterArray(array[1]);
                return NegateFilterNode(child);
            }

            // Simple condition: ["field", "op", "value"]
            if (array.Length == 3
                && array[0].ValueKind == JsonValueKind.String
                && array[1].ValueKind == JsonValueKind.String)
            {
                var op = array[1].GetString() ?? "";

                // anyof: ["field", "anyof", ["v1", "v2"]] → OR group
                if (op == "anyof" && array[2].ValueKind == JsonValueKind.Array)
                    return ExpandAnyOf(array[0].GetString() ?? "", array[2]);

                // noneof: ["field", "noneof", ["v1", "v2"]] → AND group of neq
                if (op == "noneof" && array[2].ValueKind == JsonValueKind.Array)
                    return ExpandNoneOf(array[0].GetString() ?? "", array[2]);

                return CreateFilterCondition(array[0], array[1], array[2]);
            }

            // Complex filter: [conditionOrGroup, "and"/"or", conditionOrGroup, ...]
            if (array.Length >= 3)
            {
                var conditions = new List<FilterNode>();
                string? logicalOperator = null;

                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i].ValueKind == JsonValueKind.Array)
                    {
                        // Recurse — handles nested groups AND simple conditions uniformly
                        var child = ParseFilterArray(array[i]);
                        if (child != null) conditions.Add(child);
                    }
                    else if (array[i].ValueKind == JsonValueKind.String)
                    {
                        logicalOperator ??= array[i].GetString();
                    }
                }

                if (conditions.Count > 1 && !string.IsNullOrEmpty(logicalOperator))
                    return new FilterGroup { Operator = logicalOperator, Children = conditions };

                if (conditions.Count == 1)
                    return conditions[0];
            }

            return null;
        }

        private FilterNode? ExpandAnyOf(string field, JsonElement valuesArray)
        {
            var values = valuesArray.EnumerateArray()
                .Select(ExtractFilterValue)
                .Where(v => v != null)
                .ToList();

            if (values.Count == 0) return null;
            if (values.Count == 1)
                return new FilterCondition { Field = field, Operator = "eq", Value = values[0] };

            return new FilterGroup
            {
                Operator = "or",
                Children = values.Select(v => (FilterNode)new FilterCondition
                {
                    Field = field, Operator = "eq", Value = v!
                }).ToList()
            };
        }

        private FilterNode? ExpandNoneOf(string field, JsonElement valuesArray)
        {
            var values = valuesArray.EnumerateArray()
                .Select(ExtractFilterValue)
                .Where(v => v != null)
                .ToList();

            if (values.Count == 0) return null;
            if (values.Count == 1)
            {
                return new FilterCondition { Field = field, Operator = "neq", Value = values[0] };
            }

            return new FilterGroup
            {
                Operator = "and",
                Children = values.Select(v => (FilterNode)new FilterCondition
                {
                    Field = field,
                    Operator = "neq",
                    Value = v!
                }).ToList()
            };
        }

        private FilterNode? NegateFilterNode(FilterNode? node)
        {
            if (node == null) return null;

            if (node is FilterCondition condition)
            {
                var negatedOperator = InvertOperator(condition.Operator);
                if (string.IsNullOrEmpty(negatedOperator)) return null;

                return new FilterCondition
                {
                    Field = condition.Field,
                    Operator = negatedOperator,
                    Value = condition.Value
                };
            }

            if (node is FilterGroup group)
            {
                var negatedChildren = group.Children
                    .Select(NegateFilterNode)
                    .Where(child => child != null)
                    .Cast<FilterNode>()
                    .ToList();

                if (negatedChildren.Count == 0) return null;

                var negatedGroupOperator = group.Operator.Equals("and", StringComparison.OrdinalIgnoreCase)
                    ? "or"
                    : "and";

                return new FilterGroup
                {
                    Operator = negatedGroupOperator,
                    Children = negatedChildren
                };
            }

            return null;
        }

        private static string? InvertOperator(string? op)
        {
            return (op ?? string.Empty).ToLowerInvariant() switch
            {
                "eq" => "neq",
                "neq" => "eq",
                "gt" => "lte",
                "gte" => "lt",
                "lt" => "gte",
                "lte" => "gt",
                "contains" => "notcontains",
                "notcontains" => "contains",
                "startswith" => "notstartswith",
                "notstartswith" => "startswith",
                "endswith" => "notendswith",
                "notendswith" => "endswith",
                "isnull" => "notnull",
                "notnull" => "isnull",
                "isempty" => "notempty",
                "notempty" => "isempty",
                _ => null
            };
        }

        private FilterCondition? CreateFilterCondition(JsonElement fieldElement, JsonElement opElement, JsonElement valueElement)
        {
            var field = fieldElement.GetString();
            var op = opElement.GetString();
            var value = ExtractFilterValue(valueElement);

            return new FilterCondition
            {
                Field = field ?? string.Empty,
                Operator = MapDevExtremeOperator(op ?? string.Empty),
                Value = value
            };
        }

        private static object? ExtractFilterValue(JsonElement valueElement)
        {
            return valueElement.ValueKind switch
            {
                JsonValueKind.Null => null,
                JsonValueKind.String => valueElement.GetString(),
                JsonValueKind.Number => valueElement.TryGetInt32(out var i) ? i : (object)valueElement.GetDouble(),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                _ => valueElement.GetRawText()
            };
        }

        private string MapDevExtremeOperator(string dxOperator)
        {
            return dxOperator switch
            {
                "=" => "eq",
                "<>" => "neq",
                ">" => "gt",
                ">=" => "gte",
                "<" => "lt",
                "<=" => "lte",
                "contains" => "contains",
                "notcontains" => "notcontains",
                "startswith" => "startswith",
                "endswith" => "endswith",
                _ => dxOperator
            };
        }

        /// <inheritdoc/>
        public Task<DataGridResponse<object>> ExecuteAsync(IApplicationDbContext dbContext, DataGridRequest request, CancellationToken ct)
        {
            var config = Configuration;
        
            // Handle DevExtreme LoadOptions if present
            if (request.LoadOptions != null)
            {
                MapLoadOptionsToRequest(request);
            }
        
            var take = Math.Min(
                request.Take <= 0 ? 20 : request.Take,
                Math.Min(config.MaxPageSize, GridConfiguration<TProjection>.GlobalMaxPageSizeCap));
            var skip = request.Skip < 0 ? 0 : request.Skip;

            var query = Query(dbContext);
            query = ApplyRequestQueryTransforms(query, dbContext, request);
            var metadata = BuildResponseMetadata(dbContext, request);

            // Filtering
            var predicate = GridExpressionBuilder.BuildFilterPredicate(request.Filter, config);
            if (predicate != null)
                query = query.Where(predicate);

            // Search
            query = GridExpressionBuilder.ApplySearch(query, request.SearchValue, request.SearchExpr, request.SearchOperation, config);

            // Handle grouping vs regular data flow
            if (request.Group?.Count > 0)
            {
                return Task.FromResult(HandleGroupedRequest(query, request, config, take, skip, metadata));
            }
            else
            {
                return Task.FromResult(HandleRegularRequest(query, dbContext, request, config, take, skip, metadata));
            }
        }

        /// <summary>
        /// Extension point invoked on the materialized page of rows before the response is assembled,
        /// allowing derived providers to enrich/hydrate the rows (for example resolving related data).
        /// The default implementation returns the data unchanged.
        /// </summary>
        /// <param name="data">The materialized page of rows.</param>
        /// <param name="dbContext">The database context providing data access.</param>
        /// <param name="request">The current grid request.</param>
        /// <returns>The (possibly enriched) page of rows.</returns>
        protected virtual List<TProjection> HydratePage(
            List<TProjection> data,
            IApplicationDbContext dbContext,
            DataGridRequest request)
        {
            return data;
        }

        /// <summary>
        /// Extension point that produces optional metadata attached to the response's
        /// <see cref="DataGridResponse{T}.Meta"/>. The default implementation returns <see langword="null"/>.
        /// </summary>
        /// <param name="dbContext">The database context providing data access.</param>
        /// <param name="request">The current grid request.</param>
        /// <returns>The metadata object, or <see langword="null"/> when none.</returns>
        protected virtual object? BuildResponseMetadata(IApplicationDbContext dbContext, DataGridRequest request)
        {
            return null;
        }

        private DataGridResponse<object> HandleRegularRequest(IQueryable<TProjection> query, IApplicationDbContext dbContext, DataGridRequest request, GridConfiguration<TProjection> config, int take, int skip, object? metadata)
        {
            // Total count (only if requested)
            var total = 0;
            if (request.RequireTotalCount)
                total = query.Count(); // Sync count to avoid EF Core dependency in Application

            // Calculate total summaries before paging
            var totalSummaries = GridExpressionBuilder.CalculateSummaries(query, request.TotalSummary, config);

            // Sorting (explicit or default)
            var sortList = request.Sort?.Where(s => !string.IsNullOrWhiteSpace(s.Field)).ToList() ?? new();
            if (sortList.Count > 0)
            {
                query = GridExpressionBuilder.ApplySorting(query, sortList, config);
            }
            else if (config.DefaultSort.Count > 0)
            {
                query = GridExpressionBuilder.ApplySorting(query, config.DefaultSort, config);
            }

            // Paging
            query = query.Skip(skip).Take(take);

            var data = HydratePage(query.ToList(), dbContext, request);
            var response = new DataGridResponse<object>
            {
                Data = data.Cast<object>().ToList(),
                TotalCount = total,
                Summary = totalSummaries.Count > 0 ? totalSummaries.Values.ToList() : null,
                Meta = metadata
            };
            return response;
        }

        private DataGridResponse<object> HandleGroupedRequest(IQueryable<TProjection> query, DataGridRequest request, GridConfiguration<TProjection> config, int take, int skip, object? metadata)
        {
            var optimizedGroupedResponse = TryHandleSingleLevelGrouping(query, request, config, take, skip, metadata);
            if (optimizedGroupedResponse != null)
            {
                return optimizedGroupedResponse;
            }

            // For complex projections, we need to materialize first to avoid EF Core translation issues
            // Apply all filtering and search first, then materialize the results
            var materializedData = query.ToList();
        

        
            // Calculate total summaries on the full dataset before grouping
            var totalSummaries = GridExpressionBuilder.CalculateSummaries(materializedData.AsQueryable(), request.TotalSummary, config);
            // Apply multi-level grouping in memory
            var groupDescriptors = request.Group ?? new List<GroupDescriptor>();
            if (groupDescriptors.Count == 0)
            {
                return new DataGridResponse<object>
                {
                    Data = new List<object>(),
                    TotalCount = 0,
                    GroupCount = 0,
                    Summary = totalSummaries.Count > 0 ? totalSummaries.Values.ToList() : null,
                    Meta = metadata
                };
            }

            // Helper: recursively build groups for the provided level
            List<GroupedDataItem> BuildGroups(IEnumerable<object> items, int level)
            {
                var descriptor = groupDescriptors[level];
                if (!config.TryGetField(descriptor.Selector, out var fieldDesc))
                    return new List<GroupedDataItem>();

                var propertyName = fieldDesc.SourceMember ?? fieldDesc.Name;
                var propertyInfo = typeof(TProjection).GetProperty(propertyName)
                                   ?? typeof(TProjection).GetProperty(CapitalizeFirstLetter(propertyName));
                if (propertyInfo == null) return new List<GroupedDataItem>();

                var interval = ExtractGroupInterval(descriptor.GroupInterval);

                // Group by raw key value (typed), so client date filters can build correct expressions
                var grouped = items.Cast<TProjection>()
                    .GroupBy(item => ConvertIntervalGroupKey(propertyInfo.GetValue(item), interval));

                IOrderedEnumerable<IGrouping<object?, TProjection>> sorted = descriptor.Desc
                    ? grouped.OrderByDescending(g => NormalizeSortKey(g.Key))
                    : grouped.OrderBy(g => NormalizeSortKey(g.Key));

                // For top-level groups apply paging
                IEnumerable<IGrouping<object?, TProjection>> paged = sorted;
                if (level == 0)
                {
                    paged = sorted.Skip(skip).Take(take);
                }

                var results = new List<GroupedDataItem>();
                foreach (var g in paged)
                {
                    // compute summaries for this group
                    var groupSummaries = GridExpressionBuilder.CalculateSummaries(g.AsQueryable(), request.GroupSummary, config);

                    // If there are deeper group descriptors, build nested groups
                    object? itemsNode = null;
                    if (level < groupDescriptors.Count - 1)
                    {
                        if (descriptor.IsExpanded)
                        {
                            var nested = BuildGroups(g.Cast<object>(), level + 1).Cast<object>().ToList();
                            itemsNode = nested;
                        }
                    }
                    else
                    {
                        // Leaf grouping: collapsed groups must return null items
                        itemsNode = descriptor.IsExpanded ? g.Cast<object>().ToList() : null;
                    }

                    results.Add(new GroupedDataItem
                    {
                        Key = g.Key,
                        Items = itemsNode as List<object>,
                        Count = g.Count(),
                        Summary = groupSummaries.Count > 0 ? groupSummaries.Values.ToList() : null
                    });
                }

                return results;
            }

            var topGroups = BuildGroups(materializedData.Cast<object>(), 0).Cast<object>().ToList();
            var groupCountTop = topGroups.Count;
            var totalCountFromGroups = materializedData.Count;

            return new DataGridResponse<object>
            {
                Data = topGroups,
                TotalCount = totalCountFromGroups,
                GroupCount = groupCountTop,
                Summary = totalSummaries.Count > 0 ? totalSummaries.Values.ToList() : null,
                Meta = metadata
            };
        }

        private DataGridResponse<object>? TryHandleSingleLevelGrouping(
            IQueryable<TProjection> query,
            DataGridRequest request,
            GridConfiguration<TProjection> config,
            int take,
            int skip,
            object? metadata)
        {
            if (request.Group == null || request.Group.Count != 1)
                return null;

            if (request.GroupSummary?.Count > 0 || request.TotalSummary?.Count > 0)
                return null;

            var descriptor = request.Group[0];
            if (!config.TryGetField(descriptor.Selector, out var fieldDesc))
                return null;

            var propertyName = fieldDesc.SourceMember ?? fieldDesc.Name;
            var propertyInfo = typeof(TProjection).GetProperty(propertyName)
                               ?? typeof(TProjection).GetProperty(CapitalizeFirstLetter(propertyName));
            if (propertyInfo == null)
                return null;

            var selectorLambda = BuildGroupingKeySelector(propertyInfo, descriptor.GroupInterval);
            if (selectorLambda == null)
                return null;

            var keys = ExecuteKeySelection(query, selectorLambda);
            var grouped = keys
                .GroupBy(k => k)
                .Select(g => new { Key = g.Key, Count = g.Count() });

            var orderedGroups = descriptor.Desc
                ? grouped.OrderByDescending(g => NormalizeSortKey(g.Key))
                : grouped.OrderBy(g => NormalizeSortKey(g.Key));

            var totalCount = request.RequireTotalCount ? keys.Count : 0;
            var groupCount = grouped.Count();

            var pagedGroups = orderedGroups
                .Skip(skip)
                .Take(take)
                .Select(g => new GroupedDataItem
                {
                    Key = g.Key,
                    Items = descriptor.IsExpanded ? new List<object>() : null,
                    Count = g.Count,
                    Summary = null
                })
                .Cast<object>()
                .ToList();

            return new DataGridResponse<object>
            {
                Data = pagedGroups,
                TotalCount = totalCount,
                GroupCount = groupCount,
                Summary = null,
                Meta = metadata
            };
        }

        private LambdaExpression? BuildGroupingKeySelector(System.Reflection.PropertyInfo propertyInfo, object? groupInterval)
        {
            var parameter = Expression.Parameter(typeof(TProjection), "x");
            var propertyAccess = Expression.Property(parameter, propertyInfo);
            var propertyType = propertyInfo.PropertyType;
            var underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;
            var interval = ExtractGroupInterval(groupInterval);

            if ((underlyingType == typeof(DateTime) || underlyingType == typeof(DateOnly)) && !string.IsNullOrWhiteSpace(interval))
            {
                if (underlyingType == typeof(DateOnly))
                    return null;

                var datePartName = interval.ToLowerInvariant() switch
                {
                    "year" => nameof(DateTime.Year),
                    "month" => nameof(DateTime.Month),
                    "day" => nameof(DateTime.Day),
                    _ => null
                };

                if (datePartName == null)
                    return null;

                Expression keyExpression;
                if (Nullable.GetUnderlyingType(propertyType) == typeof(DateTime))
                {
                    var hasValue = Expression.Property(propertyAccess, nameof(Nullable<DateTime>.HasValue));
                    var value = Expression.Property(propertyAccess, nameof(Nullable<DateTime>.Value));
                    var datePart = Expression.Property(value, datePartName);
                    keyExpression = Expression.Condition(
                        hasValue,
                        Expression.Convert(datePart, typeof(int?)),
                        Expression.Constant(null, typeof(int?))
                    );

                    return Expression.Lambda(
                        typeof(Func<,>).MakeGenericType(typeof(TProjection), typeof(int?)),
                        keyExpression,
                        parameter);
                }

                keyExpression = Expression.Property(propertyAccess, datePartName);
                return Expression.Lambda(
                    typeof(Func<,>).MakeGenericType(typeof(TProjection), typeof(int)),
                    keyExpression,
                    parameter);
            }

            return Expression.Lambda(
                typeof(Func<,>).MakeGenericType(typeof(TProjection), propertyType),
                propertyAccess,
                parameter);
        }

        private static string? ExtractGroupInterval(object? groupInterval)
        {
            if (groupInterval == null)
                return null;

            if (groupInterval is string intervalString)
                return intervalString;

            if (groupInterval is JsonElement jsonElement)
            {
                if (jsonElement.ValueKind == JsonValueKind.String)
                    return jsonElement.GetString();

                if (jsonElement.ValueKind == JsonValueKind.Number)
                    return jsonElement.GetRawText();
            }

            return groupInterval.ToString();
        }

        private static object? ConvertIntervalGroupKey(object? rawKey, string? interval)
        {
            if (rawKey == null || string.IsNullOrWhiteSpace(interval))
                return rawKey;

            var normalized = interval.ToLowerInvariant();

            if (rawKey is DateTime dt)
            {
                return normalized switch
                {
                    "year" => dt.Year,
                    "month" => dt.Month,
                    "day" => dt.Day,
                    _ => rawKey
                };
            }

            return rawKey;
        }

        private List<object?> ExecuteKeySelection(IQueryable<TProjection> query, LambdaExpression selectorLambda)
        {
            var method = typeof(GridSourceProvider<TProjection>)
                .GetMethod(nameof(SelectGroupingKeys), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

            if (method == null)
                return new List<object?>();

            var genericMethod = method.MakeGenericMethod(selectorLambda.ReturnType);
            var result = genericMethod.Invoke(null, new object[] { query, selectorLambda });
            return result as List<object?> ?? new List<object?>();
        }

        private static List<object?> SelectGroupingKeys<TKey>(IQueryable<TProjection> query, LambdaExpression selectorLambda)
        {
            var typedSelector = (Expression<Func<TProjection, TKey>>)selectorLambda;
            return query
                .Select(typedSelector)
                .AsEnumerable()
                .Select(x => (object?)x)
                .ToList();
        }

        private static string NormalizeSortKey(object? key)
        {
            if (key == null)
                return string.Empty;

            if (key is DateTime dt)
                return dt.Ticks.ToString(CultureInfo.InvariantCulture);

            if (key is IFormattable formattable)
                return formattable.ToString(null, CultureInfo.InvariantCulture);

            return key.ToString() ?? string.Empty;
        }

        private static string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return char.ToUpper(input[0]) + input.Substring(1);
        }
    }
}
