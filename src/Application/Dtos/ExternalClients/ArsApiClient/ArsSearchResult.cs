namespace RECAMAS.Application.Dtos.ExternalClients;

/// Specs Tables 152-155 (ARS Response Fields) — one search call returns all four blocks.
public sealed record ArsSearchResult(
    ArsTcnInformation TcnInformation,
    ArsResidencyStatus? ResidencyStatus,
    IReadOnlyList<ArsResidencyApplication> ResidencyApplications,
    ArsReturnDecision? ReturnDecision);
