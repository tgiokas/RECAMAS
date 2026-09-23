using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RECAMAS.Application.Configuration;
using RECAMAS.Application.Dtos.Interoperability.Shared;
using RECAMAS.Application.Interfaces.Interoperability;

namespace RECAMAS.Infrastructure.Interoperability.Adapters;

public sealed class ArsAdapter(HttpClient httpClient, IOptions<ArsInteroperabilitySettings> options) : IExternalServiceAdapter
{
    public string ServiceName => "ARS";

    public async Task<JsonElement> ExecuteAsync(
        string operation,
        JsonElement requestData,
        ExternalRequestContext context,
        CancellationToken cancellationToken)
    {
        AdapterOperations.RequireSearch(operation);
        var query = AdapterJson.Deserialize<ExternalSearchRequest>(requestData);
        var documentNumber = query.PassportNumber ?? query.Arc;
        var path = "api/v1/Alien/immigration-applicants";
        if (!string.IsNullOrWhiteSpace(documentNumber))
            path += "/" + Uri.EscapeDataString(documentNumber);

        using var request = new HttpRequestMessage(HttpMethod.Get, new Uri(new Uri(options.Value.BaseUrl), path));
        request.Headers.TryAddWithoutValidation("api-Key", options.Value.ApiKey);
        request.Headers.TryAddWithoutValidation("X-Correlation-ID", context.CorrelationId);
        using var response = await httpClient.SendAsync(request, cancellationToken);
        await EnsureSuccessAsync(response, cancellationToken);

        List<ArsPerson> people;
        if (string.IsNullOrWhiteSpace(documentNumber))
        {
            var envelope = await response.Content.ReadFromJsonAsync<ArsEnvelope<List<ArsPerson>>>(AdapterJson.Options, cancellationToken)
                ?? throw new InvalidOperationException("ARS returned an empty response.");
            EnsureSucceeded(envelope);
            people = envelope.Data ?? [];
        }
        else
        {
            var envelope = await response.Content.ReadFromJsonAsync<ArsEnvelope<ArsPerson>>(AdapterJson.Options, cancellationToken)
                ?? throw new InvalidOperationException("ARS returned an empty response.");
            EnsureSucceeded(envelope);
            people = envelope.Data is null ? [] : [envelope.Data];
        }

        var result = people.Where(x => Matches(x, query)).Select(Map).ToList();
        return AdapterJson.Serialize<IReadOnlyList<ExternalPersonDto>>(result);
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
            return;
        await response.Content.ReadAsStringAsync(cancellationToken);
        throw new HttpRequestException("ARS returned an unsuccessful response.", null, response.StatusCode);
    }

    private static void EnsureSucceeded<T>(ArsEnvelope<T> envelope)
    {
        if (!envelope.Succeeded)
            throw new InvalidOperationException($"ARS rejected the request with code '{envelope.ErrorCode ?? "UNKNOWN"}'.");
    }

    private static bool Matches(ArsPerson person, ExternalSearchRequest query)
    {
        var dateOfBirth = ParseDate(person.ApplicantIdentity?.Dob);
        return MatchesText(person.ApplicantIdentity?.FirstName, query.FirstName)
            && MatchesText(person.ApplicantIdentity?.Surname, query.LastName)
            && MatchesText(person.ApplicantIdentity?.Nationality, query.NationalityCode)
            && MatchesText(person.TravelDocument?.DocNo, query.PassportNumber)
            && (!query.DateOfBirth.HasValue || dateOfBirth == query.DateOfBirth)
            && (!query.DateOfBirthStart.HasValue || dateOfBirth >= query.DateOfBirthStart)
            && (!query.DateOfBirthEnd.HasValue || dateOfBirth <= query.DateOfBirthEnd);
    }

    private static bool MatchesText(string? actual, string? expected) =>
        string.IsNullOrWhiteSpace(expected)
        || string.Equals(actual?.Trim(), expected.Trim(), StringComparison.OrdinalIgnoreCase);

    private static ExternalPersonDto Map(ArsPerson person) => new(
        "ARS",
        null,
        null,
        null,
        person.ApplicantIdentity?.FirstName,
        person.ApplicantIdentity?.Surname,
        ParseDate(person.ApplicantIdentity?.Dob),
        person.ApplicantIdentity?.Nationality,
        person.TravelDocument?.DocNo,
        person.ApplicantIdentity?.Gender,
        person.TravelDocument?.IssuingCountry,
        person.TravelDocument?.DocType);

    private static DateOnly? ParseDate(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;
        string[] formats = ["yyyy-MM-dd", "yyyyMMdd", "dd/MM/yyyy"];
        return DateOnly.TryParseExact(value, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date)
            ? date
            : null;
    }

    private sealed record ArsEnvelope<T>(string? ErrorCode, string? ErrorMessage, T? Data, bool Succeeded);
    private sealed record ArsPerson(ArsIdentity? ApplicantIdentity, ArsTravelDocument? TravelDocument);
    private sealed record ArsIdentity(string? Surname, string? FirstName, string? Nationality, string? OtherNationality, string? Gender, string? Dob);
    private sealed record ArsTravelDocument(string? DocNo, string? DocType, string? IssuingCountry);
}
