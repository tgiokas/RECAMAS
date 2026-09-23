namespace RECAMAS.Application.Dtos.Interoperability.JccSigningApiClient;

/// SigningIframeUrl is the "encrypted integration URL" the Specs says gets embedded so the signer never leaves RECAMAS.
public sealed record JccSigningPackageResult(string PackageId, string SigningIframeUrl);
