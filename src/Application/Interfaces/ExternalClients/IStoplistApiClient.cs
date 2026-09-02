using RECAMAS.Application.Dtos.ExternalClients;

namespace RECAMAS.Application.Interfaces;

/// IArrivalsDeparturesApiClient (Specs 9.5 )
public interface IStoplistApiClient
{
    Task<StoplistCheckResult?> CheckAsync(StoplistSearchRequest request, CancellationToken ct = default);
}
