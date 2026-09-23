namespace RECAMAS.Application.Dtos.Interoperability.ArrivalDeparture;

public sealed record ArrivalsDeparturesSearchResult(
    IReadOnlyList<ArrivalDto> Arrivals,
    IReadOnlyList<DepartureDto> Departures);
