using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.Case;

public class ReturnDecisionApprovalTcn : BaseEntity
{
    public long ReturnDecisionApprovalItemId { get; set; }
    public long TcnProfileId { get; set; }

    public ReturnDecisionApprovalItem Item { get; set; } = null!;
    public TcnProfile TcnProfile { get; set; } = null!;
}
