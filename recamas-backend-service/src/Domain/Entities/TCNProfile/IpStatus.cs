using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.TCNProfile;

/// International Protection Status — από CASS (§3.3.2.3)
public class IpStatus : BaseEntity
{
    public long TcnProfileId { get; set; }

    public IpStatusType TypeOfStatus { get; set; }      // RefugeeStatus | SubsidiaryProtection | Other
    public DateOnly? DateOfGranting { get; set; }
    public DateOnly? ExpiryDate { get; set; }
    public DateOnly? DecisionDate { get; set; }
    public IpDecisionStatus StatusDecision { get; set; } // Pending | Approved | Rejected | Revoked | Other

    public TcnProfile TcnProfile { get; set; } = null!;
}
