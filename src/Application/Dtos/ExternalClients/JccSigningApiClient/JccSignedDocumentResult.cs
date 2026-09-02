namespace RECAMAS.Application.Dtos.ExternalClients;

/// Fetched via the API after JCC calls RECAMAS's callback URL to report completion.
public sealed record JccSignedDocumentResult(
    byte[] SignedDocumentContent,
    string VerificationStatus,
    DateTimeOffset SignedAt,
    string SignatoryId);
