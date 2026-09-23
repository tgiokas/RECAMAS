using RECAMAS.Domain.Common;
using RECAMAS.Domain.Entities.TCNProfile;

namespace RECAMAS.Domain.Entities.Case;

public class MonetaryIncentiveApprovalTcn : BaseEntity
{
    public long MonetaryIncentiveApprovalItemId { get; set; }
    public long TcnProfileId { get; set; }

    public MonetaryIncentiveApprovalItem Item { get; set; } = null!;
    public TcnProfile TcnProfile { get; set; } = null!;
}
