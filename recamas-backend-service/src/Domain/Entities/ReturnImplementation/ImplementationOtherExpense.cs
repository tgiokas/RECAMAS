using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.ReturnImplementation;

/// Λοιπές δαπάνες implementation (§6.3.1.3.2)
public class ImplementationOtherExpense : BaseEntity
{
    public long ReturnImplementationId { get; set; }

    public OtherExpenseType ExpenseType { get; set; }
    public decimal ApprovedExpenseAmount { get; set; }  // Από Case
    public decimal? ActualExpenseAmount { get; set; }   // Εισάγεται κατά implementation
    public string? AttachmentsPath { get; set; }

    public ReturnImplementation ReturnImplementation { get; set; } = null!;
}
