using Shared.Domain.Common;
using Shared.Domain.Enums;

namespace ArrivalsApi.Domain.Entities;

/// <summary>
/// Arrivals/Departures record — Police Database.
/// Mirrors §9.4 Implementation Study (Tables 163-165).
/// </summary>
public class ArrivalRecord : BaseEntity
{
    // === REQUEST fields (used for search) ===
    public string Arc { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public Nationality Nationality { get; set; }
    public string? PassportNo { get; set; }
    public DateTime DateOfBirth { get; set; }

    // === RESPONSE fields (Table 165) ===
    public MovementType MovementType { get; set; }
    public DateTime MovementDate { get; set; }
    public Airport Airport { get; set; }
}
