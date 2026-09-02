using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RECAMAS.Application.Configuration;
using RECAMAS.Application.Dtos.ExternalClients;
using RECAMAS.Application.Interfaces;
using RECAMAS.Infrastructure.ApiClients;

namespace RECAMAS.Infrastructure.ExternalClients;

/// JCC Trust Services SigningHub REST API (Specs 9.6/12.3.9) — see
/// IJccSigningApiClient and JccDtos remarks: shape is provisional pending real
/// integration details from JCC. Endpoint paths below are placeholders.
public class JccSigningApiClient : ApiClientBase, IJccSigningApiClient
{
    private const string TokenEndpoint = "/oauth/token";
    private const string PackagesEndpoint = "/api/packages";

    private readonly JccApiClientSettings _settings;

    public JccSigningApiClient(HttpClient httpClient, ILogger<JccSigningApiClient> logger, IOptions<JccApiClientSettings> settings)
        : base(httpClient, logger)
    {
        _settings = settings.Value;
    }

    public async Task<JccAccessTokenResult?> GetApplicationTokenAsync(CancellationToken ct = default)
    {
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "client_credentials",
            ["client_id"] = _settings.ClientId ?? string.Empty,
            ["client_secret"] = _settings.ClientSecret ?? string.Empty,
        });

        var request = new HttpRequestMessage(HttpMethod.Post, TokenEndpoint) { Content = content };
        var response = await SendRequestAsync(request, ct);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("JCC application token request returned {StatusCode}", (int)response.StatusCode);
            return null;
        }

        return await response.Content.ReadFromJsonAsync<JccAccessTokenResult>(cancellationToken: ct);
    }

    public async Task<JccSigningPackageResult?> CreateSigningPackageAsync(JccCreateSigningPackageRequest request, CancellationToken ct = default)
    {
        var token = await GetApplicationTokenAsync(ct);
        if (token is null)
        {
            return null;
        }

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, PackagesEndpoint)
        {
            Content = JsonContent.Create(request),
        };
        httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

        var response = await SendRequestAsync(httpRequest, ct);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("JCC create-signing-package returned {StatusCode} for document '{DocumentName}'",
                (int)response.StatusCode, request.DocumentName);
            return null;
        }

        return await response.Content.ReadFromJsonAsync<JccSigningPackageResult>(cancellationToken: ct);
    }

    public async Task<JccSignedDocumentResult?> GetSignedDocumentAsync(string packageId, CancellationToken ct = default)
    {
        var token = await GetApplicationTokenAsync(ct);
        if (token is null)
        {
            return null;
        }

        var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{PackagesEndpoint}/{packageId}/signed-document");
        httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

        var response = await SendRequestAsync(httpRequest, ct);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("JCC get-signed-document returned {StatusCode} for package {PackageId}", (int)response.StatusCode, packageId);
            return null;
        }

        return await response.Content.ReadFromJsonAsync<JccSignedDocumentResult>(cancellationToken: ct);
    }
}
