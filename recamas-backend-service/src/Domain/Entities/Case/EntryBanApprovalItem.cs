using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.Case;

/// Approval item για entry ban (§4.4.2.4.2 / §4.5.2.4.3)
public class EntryBanApprovalItem : ApprovalItem
{
    public int EntryBanDurationMonths { get; set; }
    public string? Notes { get; set; }
}
