using System.Linq.Expressions;
using RECAMAS.Application.Grids.Abstractions;
using RECAMAS.Application.Grids.Filters;
using RECAMAS.Application.Grids.Models;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace RECAMAS.Application.Grids.Helpers;

/// <summary>
/// Translates the framework's filter/search/sort/group/summary models into LINQ
/// <see cref="Expression"/> trees and applies them to an <see cref="IQueryable{T}"/>. Field names
/// are resolved against the grid's <see cref="GridConfiguration{T}"/>; unknown or disabled fields are
/// ignored, and values are coerced to each field's CLR type (with date normalization for date-only
/// inputs). String comparisons are made case-insensitive by upper-casing both sides.
/// </summary>
internal static class GridExpressionBuilder
{
    /// <summary>
    /// Builds a filter predicate from a <see cref="FilterNode"/> tree for the given projection type.
    /// </summary>
    /// <typeparam name="T">The projection (row) type the predicate applies to.</typeparam>
    /// <param name="node">The root filter node, or <see langword="null"/> for no filter.</param>
    /// <param name="config">The grid configuration used to resolve and validate fields.</param>
    /// <returns>
    /// A predicate expression, or <see langword="null"/> when <paramref name="node"/> is <see langword="null"/>
    /// or yields no applicable conditions.
    /// </returns>
    public static Expression<Func<T, bool>>? BuildFilterPredicate<T>(FilterNode? node, GridConfiguration<T> config)
    {
        if (node is null)
        {
            return null;
        }

        var param = Expression.Parameter(typeof(T), "p");
        var body = BuildNode(node, param, config);

        return body is null ? null : Expression.Lambda<Func<T, bool>>(body, param);
    }

    private static Expression? BuildNode<T>(FilterNode node, ParameterExpression p, GridConfiguration<T> config)
    {
        return node switch
        {
            FilterCondition c => BuildCondition(c, p, config),
            FilterGroup g => BuildGroup(g, p, config),
            _ => null
        };
    }

    private static Expression? BuildGroup<T>(FilterGroup group, ParameterExpression p, GridConfiguration<T> config)
    {
        if (group.Children.Count == 0) return null;
        Expression? agg = null;
        foreach (var child in group.Children)
        {
            var expr = BuildNode(child, p, config);
            if (expr is null) continue;
            if (agg is null)
            {
                agg = expr;
            }
            else
            {
                agg = group.Operator.Equals("and", StringComparison.OrdinalIgnoreCase)
                    ? Expression.AndAlso(agg, expr)
                    : Expression.OrElse(agg, expr);
            }
        }

        return agg;
    }

    private static Expression? BuildCondition<T>(FilterCondition condition, ParameterExpression p,
        GridConfiguration<T> config)
    {
        if (!config.TryGetField(condition.Field, out var field) || !field.Filterable)
        {
            return null; // Unknown or not filterable field ignored (could throw instead)
        }

        var member = Expression.PropertyOrField(p, field.SourceMember ?? field.Name);

        // Defensive: if incoming value is clearly invalid for date parsing (DevExtreme sometimes sends NaN-like placeholders)
        bool LooksLikeInvalidDate(object? inputValue)
        {
            if (inputValue is null)
            {
                return false;
            }

            if (inputValue is System.Text.Json.JsonElement element &&
                element.ValueKind == System.Text.Json.JsonValueKind.String)
            {
                var value = element.GetString();
                return !string.IsNullOrEmpty(value) && value.Contains("NaN", StringComparison.OrdinalIgnoreCase);
            }

            if (inputValue is string dateStringValue)
            {
                return !string.IsNullOrEmpty(dateStringValue) &&
                       dateStringValue.Contains("NaN", StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }

        if (LooksLikeInvalidDate(condition.Value) &&
            (Nullable.GetUnderlyingType(field.DataType) ?? field.DataType) == typeof(DateTime))
        {
            // Skip this condition quietly; it's an invalid date placeholder from the client
            return null;
        }

        // Normalize date-only strings for "lt" operator to include whole days
        var adjustedValue = condition.Value;
        if (condition.Value is string strValue && 
            (Nullable.GetUnderlyingType(field.DataType) ?? field.DataType) == typeof(DateTime) &&
            condition.Operator.ToLowerInvariant() == "lt" &&
            IsDateOnlyString(strValue))
        {
            // Parse the date and add one day
            if (DateTime.TryParse(strValue, out var dt))
            {
                adjustedValue = dt.AddDays(1).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); // ISO format
            }
        }

        Expression constant;
        try
        {
            constant = ConvertValue(adjustedValue, field.DataType);
        }
        catch (FormatException)
        {
            // Could not convert the provided value to the field's data type (e.g. invalid date string)
            // Silently ignore this filter node so the rest of the query can proceed.
            return null;
        }

        return condition.Operator.ToLowerInvariant() switch
        {
            "eq" => BuildEqualsExpression(member, constant, field.DataType),
            "neq" => BuildNotEqualsExpression(member, constant, field.DataType),
            "gt" => Expression.GreaterThan(member, constant),
            "gte" => Expression.GreaterThanOrEqual(member, constant),
            "lt" => Expression.LessThan(member, constant),
            "lte" => Expression.LessThanOrEqual(member, constant),
            // between expects an array/object with two bounds; inclusive range
            "between" => BuildBetweenExpression(member, condition.Value, field.DataType),
            "contains" => BuildStringCall(member, constant, nameof(string.Contains)),
            // Negation of contains
            "notcontains" => Expression.Not(BuildStringCall(member, constant, nameof(string.Contains))),
            "startswith" => BuildStringCall(member, constant, nameof(string.StartsWith)),
            "notstartswith" => Expression.Not(BuildStringCall(member, constant, nameof(string.StartsWith))),
            "endswith" => BuildStringCall(member, constant, nameof(string.EndsWith)),
            "notendswith" => Expression.Not(BuildStringCall(member, constant, nameof(string.EndsWith))),
            "isnull" => Expression.Equal(member, Expression.Constant(null)),
            "notnull" => Expression.NotEqual(member, Expression.Constant(null)),
            "isempty" => Expression.Equal(member, Expression.Constant(string.Empty)),
            "notempty" => Expression.NotEqual(member, Expression.Constant(string.Empty)),
            _ => null
        };
    }

    private static bool IsDateOnlyString(string value)
    {
        // Check if string matches YYYY-MM-DD format (date-only)
        return Regex.IsMatch(value, @"^\d{4}-\d{2}-\d{2}$");
    }

    private static Expression? BuildBetweenExpression(Expression member, object? value, Type fieldType)
    {
        if (value == null) return null;

        // Expect either a JsonElement array [low, high] or an object with properties { from, to } or { low, high }
        object? lowRaw = null;
        object? highRaw = null;
        switch (value)
        {
            // Handle JsonElement directly
            case System.Text.Json.JsonElement element:
                switch (element.ValueKind)
                {
                    case System.Text.Json.JsonValueKind.Array when element.GetArrayLength() >= 2:
                        lowRaw = element[0];
                        highRaw = element[1];
                        break;
                    case System.Text.Json.JsonValueKind.Object:
                    {
                        if (element.TryGetProperty("from", out var from)) lowRaw = from;
                        else if (element.TryGetProperty("low", out var low)) lowRaw = low;

                        if (element.TryGetProperty("to", out var to)) highRaw = to;
                        else if (element.TryGetProperty("high", out var high)) highRaw = high;
                        break;
                    }
                }

                break;
            // Handle stringified JSON like "[1,2]" or "{\"from\":1,\"to\":2}"
            case string s when !string.IsNullOrWhiteSpace(s):
            {
                var trimmed = s.Trim();
                if ((trimmed.StartsWith("[") && trimmed.EndsWith("]")) || (trimmed.StartsWith("{") && trimmed.EndsWith("}")))
                {
                    try
                    {
                        using var doc = System.Text.Json.JsonDocument.Parse(trimmed);
                        var root = doc.RootElement;
                        switch (root.ValueKind)
                        {
                            case System.Text.Json.JsonValueKind.Array when root.GetArrayLength() >= 2:
                                lowRaw = root[0];
                                highRaw = root[1];
                                break;
                            case System.Text.Json.JsonValueKind.Object:
                            {
                                if (root.TryGetProperty("from", out var from)) lowRaw = from;
                                else if (root.TryGetProperty("low", out var low)) lowRaw = low;

                                if (root.TryGetProperty("to", out var to)) highRaw = to;
                                else if (root.TryGetProperty("high", out var high)) highRaw = high;
                                break;
                            }
                        }
                    }
                    catch
                    {
                        // ignore parse errors
                    }
                }

                break;
            }
            // Handle plain object arrays like object[] or IEnumerable<object>
            case System.Collections.IEnumerable enumerable:
            {
                var list = enumerable.Cast<object?>().ToList();
                if (list.Count >= 2)
                {
                    lowRaw = list[0];
                    highRaw = list[1];
                }

                break;
            }
        }

        if (lowRaw == null || highRaw == null)
        {
            return null; // Can't build between without two bounds
        }

        // Convert both bounds to Expressions of the fieldType
        Expression lowConst;
        Expression highConst;
        try
        {
            lowConst = ConvertValue(lowRaw, fieldType);
            highConst = ConvertValue(highRaw, fieldType);
        }
        catch (FormatException)
        {
            return null; // Invalid bound format -> ignore this filter
        }

        // If both bounds are DateTime (or nullable DateTime) and appear to be date-only values,
        // treat the range as half-open [low, high+1day) so filtering by dates behaves like a day-range.
        if (lowConst.Type == typeof(DateTime) || lowConst.Type == typeof(DateTime?))
        {
            if (highConst.Type == typeof(DateTime) || highConst.Type == typeof(DateTime?))
            {
                // Try to extract constant DateTime values
                if (lowConst is ConstantExpression lowCe && highConst is ConstantExpression highCe &&
                    lowCe.Value is DateTime lowDt && highCe.Value is DateTime highDt)
                {
                    // Detect date-only by checking if time component is midnight
                    var lowIsDateOnly = lowDt.TimeOfDay == TimeSpan.Zero;
                    var highIsDateOnly = highDt.TimeOfDay == TimeSpan.Zero;

                    if (lowIsDateOnly && highIsDateOnly)
                    {
                        // Normalize to UTC and create exclusive upper bound = high + 1 day
                        var upperExclusive = highDt.AddDays(1);
                        var lowExpr = Expression.Constant(lowDt, lowConst.Type);
                        var upperExpr = Expression.Constant(upperExclusive, highConst.Type);
                        var ge = Expression.GreaterThanOrEqual(member, lowExpr);
                        var lt = Expression.LessThan(member, upperExpr);
                        return Expression.AndAlso(ge, lt);
                    }
                }
            }
        }

        var greaterOrEqual = Expression.GreaterThanOrEqual(member, lowConst);
        var lessOrEqual = Expression.LessThanOrEqual(member, highConst);
        return Expression.AndAlso(greaterOrEqual, lessOrEqual);
    }

    private static Expression ConvertValue(object? value, Type targetType)
    {
        if (value == null) return Expression.Constant(null, targetType);

        var nonNullable = Nullable.GetUnderlyingType(targetType) ?? targetType;

        // Handle JSON element conversion
        if (value is System.Text.Json.JsonElement jsonElement)
        {
            value = ConvertJsonElement(jsonElement, nonNullable);
        }

        // Enum fields: accept the underlying numeric value, the member name, or its localized
        // [Display]/[Description] text (DevExtreme's header filter sends the text rendered by a
        // column's calculateCellValue when the column has no calculateFilterExpression).
        if (nonNullable.IsEnum && value != null && value.GetType() != nonNullable)
        {
            value = ParseEnumValue(nonNullable, value);
        }

        // Handle string conversion to target type
        if (value is string str && nonNullable != typeof(string))
        {
            if (nonNullable == typeof(bool))
            {
                value = bool.Parse(str);
            }
            else if (nonNullable == typeof(int))
            {
                value = int.Parse(str);
            }
            else if (nonNullable == typeof(DateTime))
            {
                // Use TryParse to avoid exceptions for invalid placeholders like DevExtreme's NaN tokens
                if (!DateTime.TryParse(str, out var dt))
                {
                    throw new FormatException($"Invalid DateTime string: '{str}'");
                }

                dt = dt.Kind switch
                {
                    // Normalize to UTC to satisfy Npgsql timestamp with time zone expectations
                    DateTimeKind.Unspecified => DateTime.SpecifyKind(dt, DateTimeKind.Utc),
                    DateTimeKind.Local => dt.ToUniversalTime(),
                    _ => dt
                };
                value = dt;
            }
            else if (nonNullable == typeof(decimal))
            {
                value = decimal.Parse(str);
            }
            else
            {
                value = Convert.ChangeType(str, nonNullable);
            }
        }
        else if (value?.GetType() != nonNullable)
        {
            value = Convert.ChangeType(value, nonNullable);
        }

        // Ensure DateTime values are UTC when constructing constants so provider (Npgsql) can generate tz literals
        if (value is DateTime dtVal)
        {
            value = dtVal.Kind switch
            {
                DateTimeKind.Unspecified => DateTime.SpecifyKind(dtVal, DateTimeKind.Utc),
                DateTimeKind.Local => dtVal.ToUniversalTime(),
                _ => value
            };
        }

        return Expression.Constant(value, targetType);
    }

    /// <summary>
    /// Resolves a raw filter value (numeric code, member name, or localized <see cref="DisplayAttribute.Name"/>/
    /// <see cref="DescriptionAttribute"/> text) into an instance of the given enum type.
    /// </summary>
    /// <param name="enumType">The (non-nullable) enum type to resolve into.</param>
    /// <param name="value">The raw filter value.</param>
    /// <returns>The matching enum value.</returns>
    /// <exception cref="FormatException">Thrown when the value does not match any enum member.</exception>
    private static object ParseEnumValue(Type enumType, object value)
    {
        switch (value)
        {
            case sbyte or byte or short or ushort or int or uint or long or ulong:
                return Enum.ToObject(enumType, value);
            case string str:
            {
                if (Enum.TryParse(enumType, str, ignoreCase: true, out var byName))
                {
                    return byName!;
                }

                foreach (var member in Enum.GetValues(enumType))
                {
                    var field = enumType.GetField(member.ToString()!);
                    var label = field?.GetCustomAttribute<DisplayAttribute>()?.Name
                        ?? field?.GetCustomAttribute<DescriptionAttribute>()?.Description;
                    if (label != null && string.Equals(label, str, StringComparison.OrdinalIgnoreCase))
                    {
                        return member;
                    }
                }

                throw new FormatException($"Value '{str}' does not match any member of enum '{enumType.Name}'.");
            }
            default:
                throw new FormatException($"Cannot convert value of type '{value.GetType().Name}' to enum '{enumType.Name}'.");
        }
    }

    private static object? ConvertJsonElement(System.Text.Json.JsonElement element, Type targetType)
    {
        var nonNullable = Nullable.GetUnderlyingType(targetType) ?? targetType;

        if (nonNullable == typeof(string))
        {
            return element.GetString();
        }

        if (nonNullable == typeof(int))
        {
            return element.GetInt32();
        }

        if (nonNullable == typeof(bool))
        {
            return element.GetBoolean();
        }

        if (nonNullable == typeof(DateTime))
        {
            // Prefer TryGetDateTime and fallback to TryParse from string
            if (element.TryGetDateTime(out var dt)) return dt;
            if (element.ValueKind == System.Text.Json.JsonValueKind.String)
            {
                var s = element.GetString();
                if (!string.IsNullOrEmpty(s) && DateTime.TryParse(s, out var parsed))
                {
                    return parsed;
                }
            }

            throw new FormatException($"Invalid DateTime JSON element: '{element.GetRawText()}'");
        }

        if (nonNullable == typeof(decimal))
        {
            return element.GetDecimal();
        }

        if (nonNullable == typeof(double))
        {
            return element.GetDouble();
        }

        if (nonNullable == typeof(float))
        {
            return element.GetSingle();
        }

        if (nonNullable == typeof(long))
        {
            return element.GetInt64();
        }

        if (nonNullable.IsEnum)
        {
            return element.ValueKind switch
            {
                System.Text.Json.JsonValueKind.Number => Enum.ToObject(nonNullable, element.GetInt64()),
                System.Text.Json.JsonValueKind.String => ParseEnumValue(nonNullable, element.GetString() ?? string.Empty),
                _ => throw new FormatException($"Invalid enum JSON element for '{nonNullable.Name}': '{element.GetRawText()}'")
            };
        }

        return element.GetString();
    }

    private static Expression BuildStringCall(Expression member, Expression constant, string method)
    {
        var normalizedMember = NormalizeToUpperString(member);
        var normalizedConstant = NormalizeToUpperString(constant);

        return Expression.Call(normalizedMember, method, Type.EmptyTypes, normalizedConstant);
    }

    /// <summary>
    /// Applies a case-insensitive free-text search to the query, OR-combining the configured
    /// searchable fields.
    /// </summary>
    /// <typeparam name="T">The projection (row) type being queried.</typeparam>
    /// <param name="query">The source query.</param>
    /// <param name="search">The search value; when null/whitespace the query is returned unchanged.</param>
    /// <param name="searchExpr">Specific fields to search in; when null/empty, all searchable fields are used.</param>
    /// <param name="searchOperation">The search operation (for example <c>contains</c>, <c>startswith</c>, <c>endswith</c>, <c>eq</c>, <c>neq</c>); defaults to <c>contains</c>.</param>
    /// <param name="config">The grid configuration used to resolve searchable fields.</param>
    /// <returns>The query with the search predicate applied, or the original query if nothing is searchable.</returns>
    public static IQueryable<T> ApplySearch<T>(IQueryable<T> query, string? search, IReadOnlyList<string>? searchExpr,
        string? searchOperation, GridConfiguration<T> config)
    {
        if (string.IsNullOrWhiteSpace(search))
        {
            return query;
        }

        var p = Expression.Parameter(typeof(T), "p");
        Expression? searchExpression = null;

        // If specific fields are provided via SearchExpr, use only those fields
        if (searchExpr?.Count > 0)
        {
            var searchableFields = searchExpr
                .Where(fieldName => config.TryGetField(fieldName, out var field) && field.Searchable)
                .Select(fieldName => config.Fields[fieldName])
                .ToList();

            if (searchableFields.Count == 0) return query;

            foreach (var field in searchableFields)
            {
                var member = Expression.PropertyOrField(p, field.SourceMember ?? field.Name);
                var searchCondition = BuildSearchCondition(member, search, searchOperation);

                if (searchCondition != null)
                {
                    searchExpression = searchExpression == null
                        ? searchCondition
                        : Expression.OrElse(searchExpression, searchCondition);
                }
            }
        }
        else
        {
            // Fallback to all searchable fields (backward compatibility)
            var searchable = config.Fields.Values.Where(f => f.Searchable).ToList();
            if (searchable.Count == 0)
            {
                return query;
            }

            foreach (var field in searchable)
            {
                var member = Expression.PropertyOrField(p, field.SourceMember ?? field.Name);
                var searchCondition = BuildSearchCondition(member, search, searchOperation);

                if (searchCondition != null)
                {
                    searchExpression = searchExpression == null
                        ? searchCondition
                        : Expression.OrElse(searchExpression, searchCondition);
                }
            }
        }

        if (searchExpression == null)
        {
            return query;
        }

        var lambda = Expression.Lambda<Func<T, bool>>(searchExpression, p);
        return query.Where(lambda);
    }

    private static Expression? BuildSearchCondition(Expression member, string searchValue, string? searchOperation)
    {
        // Convert members to uppercase strings so search is case-insensitive.
        var memberString = NormalizeToUpperString(member);
        var normalizedSearchConstant = Expression.Constant(searchValue.ToUpperInvariant());

        return (searchOperation?.ToLowerInvariant()) switch
        {
            "contains" or null => Expression.Call(memberString, nameof(string.Contains), Type.EmptyTypes,
                normalizedSearchConstant),
            "startswith" => Expression.Call(memberString, nameof(string.StartsWith), Type.EmptyTypes,
                normalizedSearchConstant),
            "endswith" => Expression.Call(memberString, nameof(string.EndsWith), Type.EmptyTypes,
                normalizedSearchConstant),
            "=" or "eq" => Expression.Equal(memberString, normalizedSearchConstant),
            "!=" or "neq" => Expression.NotEqual(memberString, normalizedSearchConstant),
            _ => Expression.Call(memberString, nameof(string.Contains), Type.EmptyTypes,
                normalizedSearchConstant) // Default fallback
        };
    }

    private static Expression BuildEqualsExpression(Expression member, Expression constant, Type fieldType)
    {
        var nonNullable = Nullable.GetUnderlyingType(fieldType) ?? fieldType;
        if (nonNullable == typeof(string))
        {
            return Expression.Equal(NormalizeToUpperString(member), NormalizeToUpperString(constant));
        }

        return Expression.Equal(member, constant);
    }

    private static Expression BuildNotEqualsExpression(Expression member, Expression constant, Type fieldType)
    {
        var nonNullable = Nullable.GetUnderlyingType(fieldType) ?? fieldType;
        if (nonNullable == typeof(string))
        {
            return Expression.NotEqual(NormalizeToUpperString(member), NormalizeToUpperString(constant));
        }

        return Expression.NotEqual(member, constant);
    }

    private static Expression NormalizeToUpperString(Expression expression)
    {
        var stringExpression = expression.Type == typeof(string)
            ? expression
            : Expression.Call(expression, "ToString", Type.EmptyTypes);

        return Expression.Call(stringExpression, nameof(string.ToUpper), Type.EmptyTypes);
    }

    /// <summary>
    /// Applies the supplied sort descriptors to the query, building a multi-level ordering via
    /// <c>OrderBy</c>/<c>ThenBy</c> (and their descending variants). Unknown or non-sortable fields are skipped.
    /// </summary>
    /// <typeparam name="T">The projection (row) type being queried.</typeparam>
    /// <param name="query">The source query.</param>
    /// <param name="sort">The ordered list of sort descriptors; when empty the query is returned unchanged so the caller can apply defaults.</param>
    /// <param name="config">The grid configuration used to resolve and validate sortable fields.</param>
    /// <returns>The ordered query, or the original query when no applicable sort fields were found.</returns>
    public static IQueryable<T> ApplySorting<T>(IQueryable<T> query, IReadOnlyList<SortDescriptor> sort,
        GridConfiguration<T> config)
    {
        if (sort.Count == 0) return query; // Caller will apply defaults
        IOrderedQueryable<T>? ordered = null;
        foreach (var s in sort)
        {
            if (!config.TryGetField(s.Field, out var field) || !field.Sortable) continue;
            var p = Expression.Parameter(typeof(T), "p");
            var body = Expression.PropertyOrField(p, field.SourceMember ?? field.Name);
            var keySelector = Expression.Lambda(body, p);
            var method = ordered == null
                ? s.Desc ? "OrderByDescending" : "OrderBy"
                : s.Desc
                    ? "ThenByDescending"
                    : "ThenBy";

            ordered = (IOrderedQueryable<T>)typeof(Queryable)
                .GetMethods()
                .First(m => m.Name == method && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), body.Type)
                .Invoke(null, new object[] { ordered ?? query, keySelector })!;
        }

        return ordered ?? query;
    }

    /// <summary>
    /// Groups the query by the first supplied group descriptor (single-level grouping), projecting the
    /// key to <see cref="object"/> and ordering groups by their key. When no usable descriptor is
    /// supplied, all rows are placed in a single group.
    /// </summary>
    /// <typeparam name="T">The projection (row) type being queried.</typeparam>
    /// <param name="query">The source query.</param>
    /// <param name="groupDescriptors">The group descriptors; only the first is honored.</param>
    /// <param name="config">The grid configuration used to resolve the group field.</param>
    /// <returns>The grouped (and key-ordered) query.</returns>
    public static IQueryable<IGrouping<object, T>> ApplyGrouping<T>(IQueryable<T> query,
        IReadOnlyList<GroupDescriptor>? groupDescriptors, GridConfiguration<T> config)
    {
        if (groupDescriptors?.Count == 0)
        {
            return query.GroupBy(x => new object()); // No grouping
        }

        // For now, support single-level grouping. Multi-level grouping would require more complex logic
        var firstGroup = groupDescriptors?.FirstOrDefault();
        if (firstGroup == null || !config.TryGetField(firstGroup.Selector, out var field))
        {
            return query.GroupBy(x => new object()); // Default grouping
        }

        var parameter = Expression.Parameter(typeof(T), "p");
        var body = Expression.PropertyOrField(parameter, field.SourceMember ?? field.Name);

        // Convert to object for uniform grouping key type
        var convertToObject = Expression.Convert(body, typeof(object));
        var keySelector = Expression.Lambda<Func<T, object>>(convertToObject, parameter);

        var grouped = query.GroupBy(keySelector);

        // Apply sorting to groups if needed
        return firstGroup.Desc ? grouped.OrderByDescending(g => g.Key) : grouped.OrderBy(g => g.Key);
    }

    /// <summary>
    /// Computes the requested summary aggregations (<c>sum</c>, <c>avg</c>, <c>min</c>, <c>max</c>, <c>count</c>)
    /// over the query. Results are keyed by <c>"{selector}_{summaryType}"</c> so multiple aggregations on the
    /// same field do not overwrite each other. Unknown fields and aggregations that fail (for example a
    /// numeric aggregation on a non-numeric field) are skipped.
    /// </summary>
    /// <typeparam name="T">The projection (row) type being queried.</typeparam>
    /// <param name="query">The source query to aggregate.</param>
    /// <param name="summaryDescriptors">The summary descriptors to compute; null/empty yields an empty result.</param>
    /// <param name="config">The grid configuration used to resolve the summary fields.</param>
    /// <returns>A dictionary mapping each composite summary key to its computed value.</returns>
    public static Dictionary<string, object> CalculateSummaries<T>(IQueryable<T> query,
        IReadOnlyList<SummaryDescriptor>? summaryDescriptors, GridConfiguration<T> config)
    {
        var results = new Dictionary<string, object>();

        if (summaryDescriptors?.Count == 0)
        {
            return results;
        }

        foreach (var summary in summaryDescriptors ?? Enumerable.Empty<SummaryDescriptor>())
        {
            if (!config.TryGetField(summary.Selector, out var field)) continue;

            var parameter = Expression.Parameter(typeof(T), "p");
            var member = Expression.PropertyOrField(parameter, field.SourceMember ?? field.Name);

            try
            {
                var result = summary.SummaryType?.ToLowerInvariant() switch
                {
                    "sum" => CalculateSum(query, member, parameter),
                    "avg" => CalculateAverage(query, member, parameter),
                    "min" => CalculateMin(query, member, parameter),
                    "max" => CalculateMax(query, member, parameter),
                    "count" => query.Count(),
                    _ => null
                };

                if (result != null)
                {
                    // Use composite key to avoid overwriting multiple summaries for same field
                    var key = $"{summary.Selector}_{summary.SummaryType}";
                    results[key] = result;
                }
            }
            catch
            {
                // Skip invalid summaries (e.g., sum on non-numeric field)
            }
        }

        return results;
    }

    private static object? CalculateSum<T>(IQueryable<T> query, Expression member, ParameterExpression p)
    {
        var memberType = member.Type;
        var underlyingType = Nullable.GetUnderlyingType(memberType) ?? memberType;

        if (underlyingType == typeof(int))
        {
            var selector = Expression.Lambda<Func<T, int?>>(Expression.Convert(member, typeof(int?)), p);
            return query.Sum(selector);
        }

        if (underlyingType == typeof(decimal))
        {
            var selector = Expression.Lambda<Func<T, decimal?>>(Expression.Convert(member, typeof(decimal?)), p);
            return query.Sum(selector);
        }

        if (underlyingType == typeof(double))
        {
            var selector = Expression.Lambda<Func<T, double?>>(Expression.Convert(member, typeof(double?)), p);
            return query.Sum(selector);
        }

        if (underlyingType == typeof(float))
        {
            var selector = Expression.Lambda<Func<T, float?>>(Expression.Convert(member, typeof(float?)), p);
            return query.Sum(selector);
        }

        return null;
    }

    private static object? CalculateAverage<T>(IQueryable<T> query, Expression member, ParameterExpression parameter)
    {
        var memberType = member.Type;
        var underlyingType = Nullable.GetUnderlyingType(memberType) ?? memberType;

        if (underlyingType == typeof(int))
        {
            var selector = Expression.Lambda<Func<T, int?>>(Expression.Convert(member, typeof(int?)), parameter);
            return query.Average(selector);
        }

        if (underlyingType == typeof(decimal))
        {
            var selector = Expression.Lambda<Func<T, decimal?>>(Expression.Convert(member, typeof(decimal?)), parameter);
            return query.Average(selector);
        }

        if (underlyingType == typeof(double))
        {
            var selector = Expression.Lambda<Func<T, double?>>(Expression.Convert(member, typeof(double?)), parameter);
            return query.Average(selector);
        }

        if (underlyingType == typeof(float))
        {
            var selector = Expression.Lambda<Func<T, float?>>(Expression.Convert(member, typeof(float?)), parameter);
            return query.Average(selector);
        }

        return null;
    }

    private static object? CalculateMin<T>(IQueryable<T> query, Expression member, ParameterExpression parameter)
    {
        var memberType = member.Type;
        var underlyingType = Nullable.GetUnderlyingType(memberType) ?? memberType;

        if (underlyingType == typeof(int))
        {
            var selector = Expression.Lambda<Func<T, int?>>(Expression.Convert(member, typeof(int?)), parameter);
            return query.Min(selector);
        }

        if (underlyingType == typeof(decimal))
        {
            var selector = Expression.Lambda<Func<T, decimal?>>(Expression.Convert(member, typeof(decimal?)), parameter);
            return query.Min(selector);
        }

        if (underlyingType == typeof(double))
        {
            var selector = Expression.Lambda<Func<T, double?>>(Expression.Convert(member, typeof(double?)), parameter);
            return query.Min(selector);
        }

        if (underlyingType == typeof(float))
        {
            var selector = Expression.Lambda<Func<T, float?>>(Expression.Convert(member, typeof(float?)), parameter);
            return query.Min(selector);
        }

        return null;
    }

    private static object? CalculateMax<T>(IQueryable<T> query, Expression member, ParameterExpression parameter)
    {
        var memberType = member.Type;
        var underlyingType = Nullable.GetUnderlyingType(memberType) ?? memberType;

        if (underlyingType == typeof(int))
        {
            var selector = Expression.Lambda<Func<T, int?>>(Expression.Convert(member, typeof(int?)), parameter);
            return query.Max(selector);
        }

        if (underlyingType == typeof(decimal))
        {
            var selector = Expression.Lambda<Func<T, decimal?>>(Expression.Convert(member, typeof(decimal?)), parameter);
            return query.Max(selector);
        }

        if (underlyingType == typeof(double))
        {
            var selector = Expression.Lambda<Func<T, double?>>(Expression.Convert(member, typeof(double?)), parameter);
            return query.Max(selector);
        }

        if (underlyingType == typeof(float))
        {
            var selector = Expression.Lambda<Func<T, float?>>(Expression.Convert(member, typeof(float?)), parameter);
            return query.Max(selector);
        }

        return null;
    }
}
