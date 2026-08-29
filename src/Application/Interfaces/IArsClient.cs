using RECAMAS.Application.Dtos.ExternalClients;

namespace RECAMAS.Application.Interfaces;

/// <summary>
/// ARS (Migration Department's main TCN/immigration-status system). Study
/// 12.3.5: synchronous API via the CY Connect gateway, ARS as OpenAPI
/// provider, RECAMAS as consumer — exact endpoint TBD during system design.
/// Automatic refresh triggers (profile/case opened, daily) are Application-
/// layer scheduling concerns, not part of this client's contract.
/// </summary>
public interface IArsClient
{
    Task<ArsSearchResult?> SearchAsync(ArsSearchRequest request, CancellationToken ct = default);
}
