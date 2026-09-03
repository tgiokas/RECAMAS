using RECAMAS.Domain.Common;

namespace RECAMAS.Domain.Entities.Detention;

/// One scheduled re-assessment milestone for a detention order. Specs 1.3.2
/// names 2, 6, and 18-month milestones. MilestoneMonths is a plain int
/// rather than an enum so a policy change in the number of months doesn't
/// need a code change — the scheduling logic that creates these rows is
/// Application-layer work, not modeled here.
public class DetentionReassessment : BaseEntity
{
    public long DetentionOrderId { get; set; }

    public int MilestoneMonths { get; set; }

    public DateOnly ScheduledDate { get; set; }
    public DateOnly? CompletedDate { get; set; }

    public string? Outcome { get; set; }
}
