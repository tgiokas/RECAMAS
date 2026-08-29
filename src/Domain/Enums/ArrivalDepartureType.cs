namespace RECAMAS.Domain.Enums;

/// <summary>
/// Persisted/domain-level counterpart to Application.Dtos.ExternalClients.ArrivalOrDeparture.
/// Deliberately a separate type even though the values mirror each other —
/// the wire DTO and the domain enum are different concerns (Domain must not
/// reference Application), mapped between the two at the Infrastructure/
/// Application boundary.
/// </summary>
public enum ArrivalDepartureType
{
    Arrival = 1,
    Departure = 2,
}
