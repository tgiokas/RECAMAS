using RECAMAS.Domain.Common;
using RECAMAS.Domain.Entities.TCNProfile;

namespace RECAMAS.Domain.Entities.Case;

public class ReturnDecisionApprovalTcn : BaseEntity
{
    public long ReturnDecisionApprovalItemId { get; set; }
    public long TcnProfileId { get; set; }

    public ReturnDecisionApprovalItem Item { get; set; } = null!;
    public TcnProfile TcnProfile { get; set; } = null!;
}
