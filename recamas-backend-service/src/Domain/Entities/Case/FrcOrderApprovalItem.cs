using RECAMAS.Domain.Enums;

namespace RECAMAS.Domain.Entities.Case;

/// Detention / Deportation Order για FRC (§4.5.2.4.1)
public class FrcOrderApprovalItem : ApprovalItem
{
    public FrcOrderType OrderType { get; set; }         // DetentionOrder | DeportationOrder
    public DocumentLanguage? DocumentLanguage { get; set; }
    public string? LegalBasis { get; set; }             // Νομική βάση — string: codelist τιμή ή free text (§4.5.2.4.1: "list of values OR free text")
    public string? OrderFilePath { get; set; }
}
