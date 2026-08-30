using RECAMAS.Domain.Common;

namespace RECAMAS.Domain.Entities.ReturnImplementation;

/// LIGHT SKELETON. Real fields come from Specs Section 6.3 — escort team
/// composition, flight/ticket planning, agreement/sign-off, departure
/// confirmation, reintegration — separately for AVR single, by-own single,
/// forced single, and forced group returns (each with its own field set).
public class ReturnImplementation : BaseEntity
{
    public long CaseId { get; set; }

    public DateOnly? DepartureDate { get; set; }
    public string? FlightNumber { get; set; }
    public bool DepartureConfirmed { get; set; }
}
