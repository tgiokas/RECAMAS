using RECAMAS.Domain.Common;
using RECAMAS.Domain.Entities.TCNProfile;

namespace RECAMAS.Domain.Entities.Case;

public class TravelDocIssuanceApprovalTcn : BaseEntity
{
    public long TravelDocIssuanceApprovalItemId { get; set; }
    public long TcnProfileId { get; set; }

    public TravelDocIssuanceApprovalItem Item { get; set; } = null!;
    public TcnProfile TcnProfile { get; set; } = null!;
}
