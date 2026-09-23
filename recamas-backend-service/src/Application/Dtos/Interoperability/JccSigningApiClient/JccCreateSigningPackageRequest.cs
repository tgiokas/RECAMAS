namespace RECAMAS.Application.Dtos.Interoperability.JccSigningApiClient;

public sealed record JccCreateSigningPackageRequest(
    byte[] DocumentContent,
    string DocumentName,
    string SignerUserId);
