namespace RECAMAS.Application.Dtos.ExternalClients;

/// Specs 9.6 (JCC Trust Services / SigningHub REST API v8.4). Genuinely
/// provisional — "the detailed integration mechanism (API credentials,
/// package/workflow configuration, field placement, callback handling) shall
/// be defined in cooperation with JCC during implementation." These DTOs
/// exist so IJccSigningClient has a shape to compile against, not because
/// the wire contract is confirmed.
public sealed record JccAccessTokenResult(string AccessToken, int ExpiresInSeconds);

public sealed record JccCreateSigningPackageRequest(
    byte[] DocumentContent,
    string DocumentName,
    string SignerUserId);

/// SigningIframeUrl is the "encrypted integration URL" the Specs says gets embedded so the signer never leaves RECAMAS.
public sealed record JccSigningPackageResult(string PackageId, string SigningIframeUrl);

/// Fetched via the API after JCC calls RECAMAS's callback URL to report completion.
public sealed record JccSignedDocumentResult(
    byte[] SignedDocumentContent,
    string VerificationStatus,
    DateTimeOffset SignedAt,
    string SignatoryId);
