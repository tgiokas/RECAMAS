using RECAMAS.Domain.Common;
using RECAMAS.Domain.Entities.TCNProfile;

namespace RECAMAS.Domain.Entities.Case;

public class ByOwnReturnDecisionTcn : BaseEntity
{
    public long ByOwnReturnDecisionIssuanceId { get; set; }
    public long TcnProfileId { get; set; }

    public ByOwnReturnDecisionIssuance Issuance { get; set; } = null!;
    public TcnProfile TcnProfile { get; set; } = null!;
}
