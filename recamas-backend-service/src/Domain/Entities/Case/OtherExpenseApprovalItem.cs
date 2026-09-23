using RECAMAS.Domain.Enums;

namespace RECAMAS.Domain.Entities.Case;

/// Approval item για λοιπές δαπάνες (§4.4.2.4.2)
public class OtherExpenseApprovalItem : ApprovalItem
{
    public OtherExpenseType ExpenseType { get; set; }   // Accommodation | Translation | Medical | Administrative | Other
    public decimal Amount { get; set; }
    public string? Notes { get; set; }
}
