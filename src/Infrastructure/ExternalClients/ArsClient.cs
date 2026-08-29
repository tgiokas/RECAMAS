using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using RECAMAS.Application.Dtos.ExternalClients;
using RECAMAS.Application.Interfaces;
using RECAMAS.Infrastructure.ApiClients;

namespace RECAMAS.Infrastructure.ExternalClients;

/// ARS via the CY Connect gateway (Study 12.3.5). Endpoint path below is a
/// placeholder — the actual route ARS/CY Connect expose "shall be determined
/// during system design" per the Study itself, not guessable from it.
/// HttpClient.BaseAddress is CyConnectSettings.BaseUrl (see
/// InfrastructureServiceRegistration), so this only needs the relative path.
public class ArsClient : ApiClientBase, IArsClient
{
    // TODO: confirm real CY Connect route for ARS search once available.
    private const string SearchEndpoint = "/ars/api/v1/tcn/search";

    public ArsClient(HttpClient httpClient, ILogger<ArsClient> logger)
        : base(httpClient, logger)
    {
    }

    public async Task<ArsSearchResult?> SearchAsync(ArsSearchRequest request, CancellationToken ct = default)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, SearchEndpoint)
        {
            Content = JsonContent.Create(request),
        };

        var response = await SendRequestAsync(httpRequest, ct);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("ARS search returned {StatusCode} for ARC {Arc}", (int)response.StatusCode, request.Arc);
            return null;
        }

        return await response.Content.ReadFromJsonAsync<ArsSearchResult>(cancellationToken: ct);
    }
}
