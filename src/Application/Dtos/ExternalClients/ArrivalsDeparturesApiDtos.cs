namespace RECAMAS.Application.Dtos.ExternalClients;

/// Specs Table 164 (Arrivals/Departures Request Fields).
public sealed record ArrivalsDeparturesSearchRequest(
    string? Arc,
    string? Name,
    string? Surname,
    string? Nationality,
    string? PassportNo,
    DateOnly? DateOfBirth);

/// Specs Table 165 (Arrivals/Departures Response Fields) — one record per
/// crossing; a search can return several. See IArrivalsDeparturesApiClient
public sealed record ArrivalsDeparturesRecord(
    ArrivalOrDeparture Direction,
    DateOnly Date,
    string? Airport);

public enum ArrivalOrDeparture
{
    Arrival = 1,
    Departure = 2,
}
