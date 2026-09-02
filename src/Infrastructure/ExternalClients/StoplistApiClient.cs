using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using RECAMAS.Application.Dtos.ExternalClients;
using RECAMAS.Application.Interfaces;
using RECAMAS.Infrastructure.ApiClients;

namespace RECAMAS.Infrastructure.ExternalClients;

/// PROVISIONAL implementation of IStoplistApiClient as a live HTTP call,
/// matching Specs 9.5's field tables. Same live-API-vs-batch-file caveat as
/// ArrivalsDeparturesApiClient (Specs 12.3.7) applies — see IStoplistApiClient remarks.
public class StoplistApiClient : ApiClientBase, IStoplistApiClient
{
    private const string CheckEndpoint = "/stoplist/api/v1/check";

    public StoplistApiClient(HttpClient httpClient, ILogger<StoplistApiClient> logger)
        : base(httpClient, logger)
    {
    }

    public async Task<StoplistCheckResult?> CheckAsync(StoplistSearchRequest request, CancellationToken ct = default)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, CheckEndpoint)
        {
            Content = JsonContent.Create(request),
        };

        var response = await SendRequestAsync(httpRequest, ct);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Stoplist check returned {StatusCode} for ARC {Arc}", (int)response.StatusCode, request.Arc);
            return null;
        }

        return await response.Content.ReadFromJsonAsync<StoplistCheckResult>(cancellationToken: ct);
    }
}
