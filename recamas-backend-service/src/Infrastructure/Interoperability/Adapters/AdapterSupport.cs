using System.Text.Json;
using System.Text.Json.Serialization;
using RECAMAS.Application.Dtos.Interoperability.Shared;

namespace RECAMAS.Infrastructure.Interoperability.Adapters;

internal static class AdapterJson
{
    internal static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() },
    };

    internal static T Deserialize<T>(JsonElement element) =>
        JsonSerializer.Deserialize<T>(element.GetRawText(), Options)
        ?? throw new ArgumentException($"The request could not be deserialized as {typeof(T).Name}.");

    internal static JsonElement Serialize<T>(T value) => JsonSerializer.SerializeToElement(value, Options);
}

internal static class AdapterOperations
{
    internal static void RequireSearch(string operation)
    {
        if (!operation.Equals("Search", StringComparison.OrdinalIgnoreCase))
            throw new NotSupportedException($"Operation '{operation}' is not supported.");
    }
}

internal static class PoliceContractValidator
{
    internal static void Validate(ExternalSearchRequest request, bool includeStoplistFields)
    {
        if (request.DateOfBirth.HasValue && (request.DateOfBirthStart.HasValue || request.DateOfBirthEnd.HasValue))
            throw new ArgumentException("Provide either dateOfBirth or a dateOfBirthStart/dateOfBirthEnd range, not both.");
        if (request.DateOfBirthStart.HasValue != request.DateOfBirthEnd.HasValue)
            throw new ArgumentException("Both dateOfBirthStart and dateOfBirthEnd are required for a date range.");
        if (request.DateOfBirthStart > request.DateOfBirthEnd)
            throw new ArgumentException("dateOfBirthStart must not be later than dateOfBirthEnd.");
        if (request.FirstName?.Length > 150 || request.LastName?.Length > 150)
            throw new ArgumentException("First and last names must not exceed 150 characters.");
        if (request.PassportNumber?.Length > 50)
            throw new ArgumentException("The travel document number must not exceed 50 characters.");

        if (!includeStoplistFields)
            return;
        if (request.TravelDocumentType is not null and not ("P" or "I"))
            throw new ArgumentException("travelDocumentType must be P or I.");
        if (request.Gender is not null and not ("M" or "F" or "U"))
            throw new ArgumentException("gender must be M, F or U.");
    }
}

internal sealed record DateOfBirthCriteria(DateOnly? ExactDate, DateOnly? StartDate, DateOnly? EndDate)
{
    internal static DateOfBirthCriteria? From(ExternalSearchRequest request) =>
        request.DateOfBirth.HasValue || request.DateOfBirthStart.HasValue || request.DateOfBirthEnd.HasValue
            ? new(request.DateOfBirth, request.DateOfBirthStart, request.DateOfBirthEnd)
            : null;
}
