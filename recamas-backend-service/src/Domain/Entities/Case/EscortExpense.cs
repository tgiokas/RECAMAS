using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;

namespace RECAMAS.Domain.Entities.Case;

/// Expense item εντός escort approval (§4.4.2.4.2)
public class EscortExpense : BaseEntity
{
    public long EscortApprovalItemId { get; set; }

    public EscortExpenseType ExpenseType { get; set; }  // Transport | Accommodation | MedicalEquipment | Other
    public decimal ExpenseAmount { get; set; }
    public string? Notes { get; set; }

    public EscortApprovalItem EscortApprovalItem { get; set; } = null!;
}
