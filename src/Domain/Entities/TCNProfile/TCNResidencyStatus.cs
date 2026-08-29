using RECAMAS.Domain.Common;

namespace RECAMAS.Domain.Entities.TCNProfile;

/// <summary>
/// Study Table 5 (Residency Status). Not explicitly a repeating list in the
/// Study, unlike Residency Applications — but modeled as history here anyway
/// (soft-delete + insert-new-on-update, never overwrite in place), matching
/// the audit-trail principle used everywhere else in this codebase. "Current"
/// status = the latest non-deleted row for the profile, ordered by CreatedAt.
/// </summary>
public class TCNResidencyStatus : BaseEntity
{
    public long TCNProfileId { get; set; }

    /// <summary>Master-data code (see TCNProfile class remarks on "Enum" fields).</summary>
    public string? PermitType { get; set; }

    public DateOnly? IssueDate { get; set; }
    public string? ResidenceCategory { get; set; }
    public string? PurposeOfResidenceRnd { get; set; }
    public DateOnly? ExpiryDate { get; set; }
    public string? Status { get; set; }
    public string? ResidencyDocumentNumber { get; set; }
}
