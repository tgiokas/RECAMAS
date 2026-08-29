using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;

namespace RECAMAS.Domain.Entities.TCNProfile;

/// <summary>Study Table 12 — "Each arrival/departure is shown as a separate entry," sourced from the Police Database.</summary>
public class TCNArrivalDeparture : BaseEntity
{
    public long TCNProfileId { get; set; }

    public ArrivalDepartureType Direction { get; set; }
    public DateOnly Date { get; set; }

    /// <summary>Master-data code (see TCNProfile class remarks on "Enum" fields).</summary>
    public string? Airport { get; set; }
}
