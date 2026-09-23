using RECAMAS.Application.Dtos.Interoperability.ArrivalDeparture;
using RECAMAS.Application.Dtos.Interoperability.StopList;

namespace RECAMAS.Application.Dtos.Interoperability.Shared;

public sealed record TcnSearchResult(
    ExternalServiceResult<IReadOnlyList<ExternalPersonDto>> Ars,
    ExternalServiceResult<ExternalPersonDto?>? Cass,
    ExternalServiceResult<StoplistSearchResult>? Stoplist,
    ExternalServiceResult<ArrivalsDeparturesSearchResult>? ArrivalsDepartures,
    IReadOnlyList<DuplicateCandidateDto> PotentialDuplicates,
    string CorrelationId);
