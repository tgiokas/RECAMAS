using RECAMAS.Domain.Enums;

namespace RECAMAS.Domain.Entities.Case;

/// Approval item για έκδοση travel document (§4.4.2.4.2)
public class TravelDocIssuanceApprovalItem : ApprovalItem
{
    public TravelDocumentType TravelDocumentType { get; set; }
    public string? Notes { get; set; }

    public ICollection<TravelDocIssuanceApprovalTcn> AffectedTcns { get; set; } = [];
}
