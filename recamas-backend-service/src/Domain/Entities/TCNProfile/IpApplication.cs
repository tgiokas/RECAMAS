using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;

namespace RECAMAS.Domain.Entities.TCNProfile;

/// Αίτηση International Protection (§3.3.2.4)
public class IpApplication : BaseEntity
{
    public long TcnProfileId { get; set; }

    public IpApplicationType TypeOfApplication { get; set; }
    public DateOnly? SubmissionDate { get; set; }
    public DateOnly? ExpiryDate { get; set; }
    public DateOnly? DecisionDate { get; set; }
    public IpDecisionStatus StatusDecision { get; set; }

    public TcnProfile TcnProfile { get; set; } = null!;
}
