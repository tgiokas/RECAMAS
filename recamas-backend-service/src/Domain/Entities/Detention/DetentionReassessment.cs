using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.Detention;

/// Επανεκτίμηση κράτησης — planned ή ad-hoc (§4.5.2.5.3)
public class DetentionReassessment : BaseEntity
{
    public long ForcedReturnCaseId { get; set; }

    public ReassessmentType ReassessmentType { get; set; } // Planned | AdHoc
    public DateOnly? PlannedDate { get; set; }
    public ReassessmentStatus Status { get; set; }      // Scheduled | Completed | Overdue
    public DateOnly? CompletionDate { get; set; }
    public long? EvaluatorUserId { get; set; }
    public long? RelatedIssueId { get; set; }           // FK → DetentionUpdateIssue
    public string? Notes { get; set; }
    public string? AssessmentReportPath { get; set; }   // Storage path

    public ForcedReturnCase ForcedReturnCase { get; set; } = null!;
}
