namespace RECAMAS.Application.Dtos.ExternalClients;

/// Study Table 164 (Arrivals/Departures Request Fields).
public sealed record ArrivalsDeparturesSearchRequest(
    string? Arc,
    string? Name,
    string? Surname,
    string? Nationality,
    string? PassportNo,
    DateOnly? DateOfBirth);

/// Study Table 165 (Arrivals/Departures Response Fields) — one record per
/// crossing; a search can return several. See IArrivalsDeparturesClient
/// remarks on the live-API-vs-batch-file contradiction (9.4 vs 12.3.6).
public sealed record ArrivalsDeparturesRecord(
    ArrivalOrDeparture Direction,
    DateOnly Date,
    string? Airport);

public enum ArrivalOrDeparture
{
    Arrival = 1,
    Departure = 2,
}
