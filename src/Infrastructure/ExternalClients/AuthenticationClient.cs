using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using RECAMAS.Application.Interfaces;
using RECAMAS.Infrastructure.ApiClients;

namespace RECAMAS.Infrastructure.ExternalClients;

/// <summary>
/// Typed HttpClient for the reused Authentication service, registered with a
/// named/typed client + Polly retry policy in InfrastructureServiceRegistration
/// (same pattern as Authentication's own Keycloak clients), on top of
/// ApiClientBase for structured request/response logging and redaction.
/// Stub only — real endpoint paths to be confirmed once we integrate for real.
/// </summary>
public class AuthenticationClient : ApiClientBase, IAuthenticationClient
{
    public AuthenticationClient(HttpClient httpClient, ILogger<AuthenticationClient> logger)
        : base(httpClient, logger)
    {
    }

    public async Task<UserSummary?> GetUserAsync(string userId, CancellationToken ct = default)
    {
        // TODO: confirm actual Authentication endpoint shape.
        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/users/{userId}");
        var response = await SendRequestAsync(request, ct);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<UserSummary>(cancellationToken: ct);
    }
}
