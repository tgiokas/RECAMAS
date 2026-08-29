using System.Net.Http.Json;
using RECAMAS.Domain.Interfaces;

namespace RECAMAS.Infrastructure.ExternalClients;

/// <summary>
/// Typed HttpClient for the reused Authentication service, registered with a
/// named/typed client + Polly retry policy in InfrastructureServiceRegistration
/// (same pattern as Authentication's own Keycloak clients).
/// Stub only — real endpoint paths to be confirmed once we integrate for real.
/// </summary>
public class AuthenticationClient : IAuthenticationClient
{
    private readonly HttpClient _httpClient;

    public AuthenticationClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<UserSummary?> GetUserAsync(string userId, CancellationToken ct = default)
    {
        // TODO: confirm actual Authentication endpoint shape.
        var response = await _httpClient.GetAsync($"/api/users/{userId}", ct);
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<UserSummary>(cancellationToken: ct);
    }
}
