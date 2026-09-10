using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.Case;

/// Approval item για χρηματικό κίνητρο (§4.4.2.4.2)
public class MonetaryIncentiveApprovalItem : ApprovalItem
{
    public decimal Amount { get; set; }                 // Pre-filled από Program/Country — editable
    public string? Notes { get; set; }

    public ICollection<MonetaryIncentiveApprovalTcn> AffectedTcns { get; set; } = [];
}
