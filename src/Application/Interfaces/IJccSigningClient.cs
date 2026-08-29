using RECAMAS.Application.Dtos.ExternalClients;

namespace RECAMAS.Application.Interfaces;

/// <summary>
/// JCC Trust Services (SigningHub REST API v8.4), Study 9.6/12.3.9. Shape is
/// provisional — see JccDtos remarks. Three-step flow: get an application
/// token, create a signing package for a document (returns an iframe URL
/// embedded directly in RECAMAS so the signer never leaves the app), then
/// retrieve the signed document once JCC's callback reports completion.
/// </summary>
public interface IJccSigningClient
{
    Task<JccAccessTokenResult?> GetApplicationTokenAsync(CancellationToken ct = default);

    Task<JccSigningPackageResult?> CreateSigningPackageAsync(JccCreateSigningPackageRequest request, CancellationToken ct = default);

    Task<JccSignedDocumentResult?> GetSignedDocumentAsync(string packageId, CancellationToken ct = default);
}
