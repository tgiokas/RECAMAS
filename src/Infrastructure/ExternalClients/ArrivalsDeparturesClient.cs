using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using RECAMAS.Application.Dtos.ExternalClients;
using RECAMAS.Application.Interfaces;
using RECAMAS.Infrastructure.ApiClients;

namespace RECAMAS.Infrastructure.ExternalClients;

/// PROVISIONAL implementation of IArrivalsDeparturesClient as a live HTTP
/// call, matching Specs 9.4's field tables. See the interface's remarks:
public class ArrivalsDeparturesClient : ApiClientBase, IArrivalsDeparturesClient
{
    private const string SearchEndpoint = "/arrivals-departures/api/v1/search";

    public ArrivalsDeparturesClient(HttpClient httpClient, ILogger<ArrivalsDeparturesClient> logger)
        : base(httpClient, logger)
    {
    }

    public async Task<IReadOnlyList<ArrivalsDeparturesRecord>?> SearchAsync(ArrivalsDeparturesSearchRequest request, CancellationToken ct = default)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, SearchEndpoint)
        {
            Content = JsonContent.Create(request),
        };

        var response = await SendRequestAsync(httpRequest, ct);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Arrivals/Departures search returned {StatusCode} for ARC {Arc}", (int)response.StatusCode, request.Arc);
            return null;
        }

        return await response.Content.ReadFromJsonAsync<List<ArrivalsDeparturesRecord>>(cancellationToken: ct);
    }
}
