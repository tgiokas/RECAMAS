using RECAMAS.Domain.Common;

namespace RECAMAS.Domain.Entities.TCNProfile;

/// Specs Table 10 — a profile-level rollup, sourced from ARS, CASS, or a
/// RECAMAS Case (IssuingAuthority "rules to be established" per the Specs
/// itself). Note VoluntaryReturnDeadline is typed "Date" here, but the ARS/
/// CASS interface DTOs (ArsReturnDecision/CassReturnDecision) type the same
/// concept as "Number" (a day count) — a real inconsistency in the source
/// document between the profile view and the interface response.
public class TCNReturnDecision : BaseEntity
{
    public long TCNProfileId { get; set; }

    /// Master-data code — ARS/CASS/RECAMAS-Case, per Source column.
    public string? IssuingAuthority { get; set; }

    public DateOnly? DecisionDate { get; set; }
    public string? DecisionText { get; set; }
    public DateOnly? TcnReceiptDate { get; set; }
    public DateOnly? VoluntaryReturnDeadline { get; set; }
    public int? EntryBanDurationMonths { get; set; }

    public Guid? DecisionFileDocumentId { get; set; }
}
