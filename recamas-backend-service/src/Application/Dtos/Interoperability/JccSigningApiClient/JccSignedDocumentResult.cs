namespace RECAMAS.Application.Dtos.Interoperability.JccSigningApiClient;

/// Fetched via the API after JCC calls RECAMAS's callback URL to report completion.
public sealed record JccSignedDocumentResult(
    byte[] SignedDocumentContent,
    string VerificationStatus,
    DateTimeOffset SignedAt,
    string SignatoryId);
