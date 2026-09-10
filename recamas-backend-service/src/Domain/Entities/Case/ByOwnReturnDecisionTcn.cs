using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.Case;

public class ByOwnReturnDecisionTcn : BaseEntity
{
    public long ByOwnReturnDecisionIssuanceId { get; set; }
    public long TcnProfileId { get; set; }

    public ByOwnReturnDecisionIssuance Issuance { get; set; } = null!;
    public TcnProfile TcnProfile { get; set; } = null!;
}
