using RECAMAS.Application.Dtos.ExternalClients;

namespace RECAMAS.Application.Interfaces;

/// <summary>
/// UNRESOLVED CONTRADICTION IN THE SOURCE REQUIREMENTS — flagged, not decided
/// here: Study 9.4 describes a live, synchronous Request/Response interface
/// (field tables given, Table 164/165) — this interface's shape assumes that.
/// But Study 12.3.6 says the real integration is "batch (asynchronous) file
/// exchange over the Police Public Zone, with the system providing an
/// automated file-import facility triggered on a definable timed basis" — a
/// fundamentally different mechanism (a scheduled file importer populating a
/// local table, not a request/response call at all).
///
/// This interface is written transport-agnostic on purpose (SearchAsync
/// returns a result regardless of how it was obtained) so calling code
/// doesn't have to change if the concrete implementation is later swapped
/// from an HTTP client (ArrivalsDeparturesClient, current placeholder) to a
/// file-import-backed repository query. Needs a decision from the director
/// before this is built for real — see session log Section 3.
/// </summary>
public interface IArrivalsDeparturesClient
{
    Task<IReadOnlyList<ArrivalsDeparturesRecord>?> SearchAsync(ArrivalsDeparturesSearchRequest request, CancellationToken ct = default);
}
