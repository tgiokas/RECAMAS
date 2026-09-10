using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.Case;

/// Approval item για έκδοση travel document (§4.4.2.4.2)
public class TravelDocIssuanceApprovalItem : ApprovalItem
{
    public TravelDocumentType TravelDocumentType { get; set; }
    public string? Notes { get; set; }

    public ICollection<TravelDocIssuanceApprovalTcn> AffectedTcns { get; set; } = [];
}
