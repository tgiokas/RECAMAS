using RECAMAS.Application.Dtos.ExternalClients;

namespace RECAMAS.Application.Interfaces;

/// Same unresolved live-API-vs-batch-file contradiction as
/// IArrivalsDeparturesApiClient (Specs 9.5 vs 12.3.7) — see its remarks. Written
/// transport-agnostic for the same reason.
public interface IStoplistApiClient
{
    Task<StoplistCheckResult?> CheckAsync(StoplistSearchRequest request, CancellationToken ct = default);
}
