namespace RECAMAS.Application.Dtos.ExternalClients;

/// Specs Table 165 (Arrivals/Departures Response Fields) — one record per
/// crossing; a search can return several. See IArrivalsDeparturesApiClient
/// remarks on the live-API-vs-batch-file contradiction (9.4 vs 12.3.6).
public sealed record ArrivalsDeparturesRecord(
    ArrivalOrDeparture Direction,
    DateOnly Date,
    string? Airport);
