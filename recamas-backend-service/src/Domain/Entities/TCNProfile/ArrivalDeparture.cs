using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;

namespace RECAMAS.Domain.Entities.TCNProfile;

/// Εγγραφή άφιξης / αναχώρησης TCN — από Police Database (§3.3.3.2)
public class ArrivalDeparture : BaseEntity
{
    public long TcnProfileId { get; set; }

    public MovementType MovementType { get; set; }      // Arrival | Departure
    public DateOnly Date { get; set; }
    public string? AirportCode { get; set; }            // IATA airport code — string (standard code, πχ. "LCA")
    public long? ExternalRecordId { get; set; }
    public long? ExternalPersonId { get; set; }
    public long? LinkedDepartureExternalId { get; set; }
    public string? PassportIssuingCountryCode { get; set; }
    public string? PassportNumber { get; set; }
    public string? VisaNumber { get; set; }
    public int? ExternalStatusCode { get; set; }
    public DataSourceType Source { get; set; } = DataSourceType.PoliceDb;
    public DateTimeOffset? LastSyncedAt { get; set; }

    public TcnProfile TcnProfile { get; set; } = null!;
}
