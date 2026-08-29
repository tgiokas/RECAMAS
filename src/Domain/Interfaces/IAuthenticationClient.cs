namespace RECAMAS.Domain.Interfaces;

/// <summary>
/// Contract for talking to the reused Authentication microservice.
/// RECAMAS's API Gateway/middleware validates JWTs directly against Keycloak
/// (no client needed for that path) — this interface is for the cases where
/// RECAMAS needs to actively call Authentication, e.g. looking up a user's
/// display name/role for an audit entry or notification recipient list.
/// Implemented in Infrastructure/ExternalClients/AuthenticationClient.cs.
/// </summary>
public interface IAuthenticationClient
{
    Task<UserSummary?> GetUserAsync(string userId, CancellationToken ct = default);
}

/// <summary>Minimal shape — expand once we know exactly what Authentication exposes.</summary>
public record UserSummary(string UserId, string DisplayName, IReadOnlyList<string> Roles);
