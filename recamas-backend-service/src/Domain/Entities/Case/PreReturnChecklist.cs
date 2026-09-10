using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.Case;

/// Pre-return checklist sign-off (§4.4.2.7)
public class PreReturnChecklist : BaseEntity
{
    public long CaseId { get; set; }                    // 1:1 με ReturnCase

    public bool SignedOff { get; set; }
    public long? SignedOffByUserId { get; set; }
    public DateTimeOffset? SignedOffAt { get; set; }

    public ReturnCase Case { get; set; } = null!;
}
