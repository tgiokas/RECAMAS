using RECAMAS.Application.Dtos.ExternalClients;

namespace RECAMAS.Application.Interfaces;

/// Specs 9.4  (Table 164/165) 

public interface IArrivalsDeparturesApiClient
{
    Task<IReadOnlyList<ArrivalsDeparturesRecord>?> SearchAsync(ArrivalsDeparturesSearchRequest request, CancellationToken ct = default);
}
