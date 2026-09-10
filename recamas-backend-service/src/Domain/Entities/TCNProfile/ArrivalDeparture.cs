using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.TCNProfile;

/// Εγγραφή άφιξης / αναχώρησης TCN — από Police Database (§3.3.3.2)
public class ArrivalDeparture : BaseEntity
{
    public long TcnProfileId { get; set; }

    public MovementType MovementType { get; set; }      // Arrival | Departure
    public DateOnly Date { get; set; }
    public string? AirportCode { get; set; }            // IATA airport code — string (standard code, πχ. "LCA")
    public DateTimeOffset? LastSyncedAt { get; set; }

    public TcnProfile TcnProfile { get; set; } = null!;
}
