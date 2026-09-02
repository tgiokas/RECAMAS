namespace RECAMAS.Application.Dtos.ExternalClients;

/// Specs 9.6 (JCC Trust Services / SigningHub REST API v8.4).
public sealed record JccAccessTokenResult(string AccessToken, int ExpiresInSeconds);
