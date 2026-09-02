using RECAMAS.Application.Dtos.ExternalClients;

namespace RECAMAS.Application.Interfaces;

/// ARS (Migration Department's main TCN/immigration-status system). Specs
/// 12.3.5: synchronous API via the CY Connect gateway, ARS as OpenAPI
/// provider, exact endpoint TBD during system design.
public interface IArsApiClient
{
    Task<ArsSearchResult?> SearchAsync(ArsSearchRequest request, CancellationToken ct = default);
}
