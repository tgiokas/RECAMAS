using System.Text.Json;
using System.Text.Json.Serialization;
using RECAMAS.Application.Grids.Filters;

namespace RECAMAS.Application.Grids.Helpers;

/// <summary>
/// <see cref="JsonConverter{T}"/> for <see cref="FilterNode"/> trees. On read, it accepts both the
/// internal object format (with an explicit <c>type</c> discriminator) and the DevExtreme filter
/// formats — array form such as <c>["field", "operator", "value"]</c>, negation <c>["!", expr]</c>,
/// header-filter <c>anyof</c>/<c>noneof</c>, and complex logical arrays — normalizing them into the
/// internal <see cref="FilterCondition"/>/<see cref="FilterGroup"/> model.
/// </summary>
public class FilterNodeJsonConverter : JsonConverter<FilterNode>
{
    /// <summary>
    /// Reads and deserializes a <see cref="FilterNode"/> from JSON, supporting both the internal
    /// typed object format and the DevExtreme array/object filter formats.
    /// </summary>
    /// <param name="reader">The JSON reader positioned at the filter value.</param>
    /// <param name="typeToConvert">The type being converted (<see cref="FilterNode"/>).</param>
    /// <param name="options">The active serializer options.</param>
    /// <returns>The deserialized filter node, or <see langword="null"/>.</returns>
    /// <exception cref="JsonException">Thrown when the JSON does not represent a recognizable filter structure.</exception>
    public override FilterNode? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        switch (root.ValueKind)
        {
            // Handle DevExtreme array format: ["field", "operator", "value"] or complex nested arrays
            case JsonValueKind.Array:
                return ParseDevExtremeArrayFormat(root);
            // Handle DevExtreme object format with nested conditions and groups
            case JsonValueKind.Object:
            {
                // Check if it's our internal format with explicit "type" property
                if (root.TryGetProperty("type", out var typeProperty))
                {
                    var type = typeProperty.GetString();
                    return type switch
                    {
                        "group" => JsonSerializer.Deserialize<FilterGroup>(root.GetRawText(), options),
                        "condition" => JsonSerializer.Deserialize<FilterCondition>(root.GetRawText(), options),
                        _ => throw new JsonException($"Unknown FilterNode type: {type}")
                    };
                }
            
                // Check if it's a DevExtreme complex filter with nested arrays
                return ParseDevExtremeComplexFormat(root);
            }
            default:
                throw new JsonException("Filter must be an array or object");
        }
    }
    
    private static FilterNode ParseDevExtremeArrayFormat(JsonElement arrayElement)
    {
        var arrayLength = arrayElement.GetArrayLength();

        // Unary negation: ["!", ["field", "=", "value"]]
        if (arrayLength == 2
            && arrayElement[0].ValueKind == JsonValueKind.String
            && arrayElement[0].GetString() == "!"
            && arrayElement[1].ValueKind == JsonValueKind.Array)
        {
            var child = ParseDevExtremeArrayFormat(arrayElement[1]);
            return NegateFilterNode(child);
        }
        
        // Simple condition: ["field", "operator", "value"]
        if (arrayLength == 3 && 
            arrayElement[0].ValueKind == JsonValueKind.String &&
            arrayElement[1].ValueKind == JsonValueKind.String)
        {
            var field = arrayElement[0].GetString() ?? string.Empty;
            var operatorStr = arrayElement[1].GetString() ?? string.Empty;

            // DevExtreme header-filter operators
            if (operatorStr.Equals("anyof", StringComparison.OrdinalIgnoreCase)
                && arrayElement[2].ValueKind == JsonValueKind.Array)
            {
                return ExpandAnyOf(field, arrayElement[2]);
            }

            if (operatorStr.Equals("noneof", StringComparison.OrdinalIgnoreCase)
                && arrayElement[2].ValueKind == JsonValueKind.Array)
            {
                return ExpandNoneOf(field, arrayElement[2]);
            }

            var value = ExtractValue(arrayElement[2]);
            
            return new FilterCondition
            {
                Field = field,
                Operator = MapDevExtremeOperator(operatorStr),
                Value = value
            };
        }
        
        // Complex filter with logical operators: [condition1, "and"/"or", condition2, ...]
        if (arrayLength >= 3)
        {
            var conditions = new List<FilterNode>();
            var logicalOperator = "and"; // Default
            
            for (int i = 0; i < arrayLength; i++)
            {
                var element = arrayElement[i];
                
                if (element.ValueKind == JsonValueKind.String)
                {
                    // This should be a logical operator like "and" or "or"
                    var op = element.GetString()?.ToLower();
                    if (op == "and" || op == "or")
                    {
                        logicalOperator = op;
                    }
                }
                else if (element.ValueKind == JsonValueKind.Array)
                {
                    // This should be a nested condition array
                    var nestedCondition = ParseDevExtremeArrayFormat(element);
                    conditions.Add(nestedCondition);
                }
            }

            return conditions.Count switch
            {
                0 => throw new JsonException("No valid conditions found in complex filter array"),
                1 => conditions[0],
                _ => new FilterGroup { Operator = logicalOperator, Children = conditions }
            };
        }
        
        throw new JsonException("DevExtreme filter array must have at least 3 elements");
    }

    private static FilterNode ExpandAnyOf(string field, JsonElement valuesArray)
    {
        var values = valuesArray.EnumerateArray()
            .Select(ExtractValue)
            .Where(v => v != null)
            .ToList();

        if (values.Count == 0)
            throw new JsonException("DevExtreme anyof filter must contain at least one value");

        if (values.Count == 1)
        {
            return new FilterCondition
            {
                Field = field,
                Operator = "eq",
                Value = values[0]
            };
        }

        return new FilterGroup
        {
            Operator = "or",
            Children = values.Select(v => (FilterNode)new FilterCondition
            {
                Field = field,
                Operator = "eq",
                Value = v
            }).ToList()
        };
    }

    private static FilterNode ExpandNoneOf(string field, JsonElement valuesArray)
    {
        var values = valuesArray.EnumerateArray()
            .Select(ExtractValue)
            .Where(v => v != null)
            .ToList();

        if (values.Count == 0)
            throw new JsonException("DevExtreme noneof filter must contain at least one value");

        if (values.Count == 1)
        {
            return new FilterCondition
            {
                Field = field,
                Operator = "neq",
                Value = values[0]
            };
        }

        return new FilterGroup
        {
            Operator = "and",
            Children = values.Select(v => (FilterNode)new FilterCondition
            {
                Field = field,
                Operator = "neq",
                Value = v
            }).ToList()
        };
    }

    private static FilterNode NegateFilterNode(FilterNode node)
    {
        return node switch
        {
            FilterCondition c => new FilterCondition
            {
                Field = c.Field,
                Operator = InvertOperator(c.Operator),
                Value = c.Value
            },
            FilterGroup g => new FilterGroup
            {
                Operator = g.Operator.Equals("and", StringComparison.OrdinalIgnoreCase) ? "or" : "and",
                Children = g.Children.Select(NegateFilterNode).ToList()
            },
            _ => throw new JsonException("Unsupported filter node type for negation")
        };
    }

    private static string InvertOperator(string op)
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
            _ => throw new JsonException($"Unsupported operator for negation: {op}")
        };
    }
    
    private static FilterNode ParseDevExtremeComplexFormat(JsonElement objectElement)
    {
        // Handle complex DevExtreme filters that might have logical operators
        // For now, treat as a simple condition if we can't determine the structure
        // This can be expanded to handle more complex DevExtreme filter structures
        
        // If it has common condition properties, treat as condition
        if (objectElement.TryGetProperty("field", out var fieldProp) && 
            objectElement.TryGetProperty("operator", out var opProp))
        {
            var field = fieldProp.GetString() ?? string.Empty;
            var operatorStr = opProp.GetString() ?? string.Empty;
            var value = objectElement.TryGetProperty("value", out var valueProp) 
                ? ExtractValue(valueProp) 
                : null;
                
            return new FilterCondition
            {
                Field = field,
                Operator = MapDevExtremeOperator(operatorStr),
                Value = value
            };
        }
        
        // Default to empty condition if we can't parse it
        return new FilterCondition();
    }
    
    private static string MapDevExtremeOperator(string devExtremeOperator)
    {
        return devExtremeOperator switch
        {
            "=" => "eq",
            "<>" => "neq",
            "!=" => "neq", 
            ">" => "gt",
            ">=" => "gte",
            "<" => "lt",
            "<=" => "lte",
            "contains" => "contains",
            "notcontains" => "notcontains",
            "startswith" => "startswith",
            "endswith" => "endswith",
            _ => devExtremeOperator.ToLowerInvariant()
        };
    }

    private static object? ExtractValue(JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.String => element.GetString(),
            JsonValueKind.Number => element.TryGetInt32(out var intVal) ? intVal : element.GetDecimal(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Null => null,
            // Preserve arrays/objects as JsonElement so callers (e.g. "between") can inspect items
            JsonValueKind.Array => element,
            JsonValueKind.Object => element,
            _ => element.GetRawText()
        };
    }

    /// <summary>
    /// Writes a <see cref="FilterNode"/> to JSON using its concrete runtime type so that the
    /// discriminating properties of <see cref="FilterCondition"/>/<see cref="FilterGroup"/> are serialized.
    /// </summary>
    /// <param name="writer">The JSON writer.</param>
    /// <param name="value">The filter node to serialize.</param>
    /// <param name="options">The active serializer options.</param>
    public override void Write(Utf8JsonWriter writer, FilterNode value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
