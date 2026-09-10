using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.Case;

/// Base class για approval items (§4.4.2.4)
public abstract class ApprovalItem : BaseEntity
{
    public string? ApprovalItemId { get; set; }          // System-generated human-readable ID — string (e.g. Order ID, Measure ID — Tables 65-69)

    public long CaseId { get; set; }

    public ApprovalItemType ItemType { get; set; }
    public ApprovalDecision AssessmentDecision { get; set; } // Pending | Approved | Rejected
    public DateTimeOffset? AssessmentDateTime { get; set; }
    public long? AssessedByUserId { get; set; }
    public string? ApproverNotes { get; set; }          // Σημειώσεις εγκριτή — free text

    // Revocation (FRC)
    public DateTimeOffset? RevocationDateTime { get; set; }
    public string? RevocationReason { get; set; }       // Λόγος ανάκλησης — free text

    public long? RelatedIssueId { get; set; }           // FK → DetentionUpdateIssue

    public ReturnCase Case { get; set; } = null!;
}
