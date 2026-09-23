using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RECAMAS.Application.Configuration;
using RECAMAS.Application.Dtos.Interoperability.ArrivalDeparture;
using RECAMAS.Application.Dtos.Interoperability.Shared;
using RECAMAS.Application.Dtos.Interoperability.StopList;
using RECAMAS.Application.Interfaces.Interoperability;

namespace RECAMAS.Infrastructure.Interoperability.Adapters;

public abstract class PoliceAdapterBase(HttpClient httpClient, IOptions<PoliceInteroperabilitySettings> options)
{
    protected async Task<T> PostAsync<T>(string path, object body, ExternalRequestContext context, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, new Uri(new Uri(options.Value.BaseUrl), path))
        {
            Content = JsonContent.Create(body, options: AdapterJson.Options),
        };
        request.Headers.TryAddWithoutValidation("api-key", options.Value.ApiKey);
        request.Headers.TryAddWithoutValidation("X-Client-ID", options.Value.ClientId);
        request.Headers.TryAddWithoutValidation("X-Correlation-ID", context.CorrelationId);

        using var response = await httpClient.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException("Police API returned an unsuccessful response.", null, response.StatusCode);
        }

        return await response.Content.ReadFromJsonAsync<T>(AdapterJson.Options, cancellationToken)
            ?? throw new InvalidOperationException("Police API returned an empty response.");
    }
}

public sealed class StoplistAdapter(HttpClient httpClient, IOptions<PoliceInteroperabilitySettings> options)
    : PoliceAdapterBase(httpClient, options), IExternalServiceAdapter
{
    public string ServiceName => "STOPLIST";

    public async Task<JsonElement> ExecuteAsync(string operation, JsonElement requestData, ExternalRequestContext context, CancellationToken cancellationToken)
    {
        AdapterOperations.RequireSearch(operation);
        var query = AdapterJson.Deserialize<ExternalSearchRequest>(requestData);
        PoliceContractValidator.Validate(query, includeStoplistFields: true);
        var body = new PoliceSearchCriteria(
            query.FirstName,
            query.LastName,
            DateOfBirthCriteria.From(query),
            query.NationalityCode,
            query.PassportNumber,
            query.TravelDocumentIssuingCountry ?? query.PassportIssuingCountry,
            query.TravelDocumentType,
            query.Gender);
        var result = await PostAsync<StoplistSearchResult>(
            "police/police-checks/v1/stoplist/search", body, context, cancellationToken);
        return AdapterJson.Serialize(result);
    }
}

public sealed class ArrivalsDeparturesAdapter(HttpClient httpClient, IOptions<PoliceInteroperabilitySettings> options)
    : PoliceAdapterBase(httpClient, options), IExternalServiceAdapter
{
    public string ServiceName => "ARRIVALS_DEPARTURES";

    public async Task<JsonElement> ExecuteAsync(string operation, JsonElement requestData, ExternalRequestContext context, CancellationToken cancellationToken)
    {
        AdapterOperations.RequireSearch(operation);
        var query = AdapterJson.Deserialize<ExternalSearchRequest>(requestData);
        PoliceContractValidator.Validate(query, includeStoplistFields: false);
        var body = new ArrivalsDeparturesCriteria(
            query.FirstName,
            query.LastName,
            DateOfBirthCriteria.From(query),
            query.NationalityCode,
            query.PassportNumber,
            query.PassportIssuingCountry ?? query.TravelDocumentIssuingCountry);
        var result = await PostAsync<PoliceMovementsResponse>(
            "police/police-checks/v1/arrivals-departures/search", body, context, cancellationToken);
        return AdapterJson.Serialize(new ArrivalsDeparturesSearchResult(result.Arrivals ?? [], result.Departures ?? []));
    }
}

internal sealed record PoliceSearchCriteria(
    string? FirstNames,
    string? LastName,
    DateOfBirthCriteria? DateOfBirth,
    string? Nationality,
    string? TravelDocumentNumber,
    string? TravelDocumentIssuingCountry,
    string? TravelDocumentType,
    string? Gender);

internal sealed record ArrivalsDeparturesCriteria(
    string? FirstNames,
    string? LastName,
    DateOfBirthCriteria? DateOfBirth,
    string? Nationality,
    string? TravelDocumentNumber,
    string? PassportIssuingCountry);

internal sealed record PoliceMovementsResponse(List<ArrivalDto>? Arrivals, List<DepartureDto>? Departures);
