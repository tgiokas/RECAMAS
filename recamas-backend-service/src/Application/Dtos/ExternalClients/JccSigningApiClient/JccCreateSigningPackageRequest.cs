namespace RECAMAS.Application.Dtos.ExternalClients;

public sealed record JccCreateSigningPackageRequest(
    byte[] DocumentContent,
    string DocumentName,
    string SignerUserId);
