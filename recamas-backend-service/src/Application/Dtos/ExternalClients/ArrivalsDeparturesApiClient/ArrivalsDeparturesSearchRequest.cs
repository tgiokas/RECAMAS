namespace RECAMAS.Application.Dtos.ExternalClients;

/// Specs Table 164 (Arrivals/Departures Request Fields).
public sealed record ArrivalsDeparturesSearchRequest(
    string? Arc,
    string? Name,
    string? Surname,
    string? Nationality,
    string? PassportNo,
    DateOnly? DateOfBirth);
