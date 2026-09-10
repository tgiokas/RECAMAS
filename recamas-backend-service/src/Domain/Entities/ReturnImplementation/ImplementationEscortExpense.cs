using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.ReturnImplementation;

/// Δαπάνη escort member (§6.3.1.3.1.3)
public class ImplementationEscortExpense : BaseEntity
{
    public long ImplementationEscortTeamMemberId { get; set; }

    public EscortExpenseType ExpenseType { get; set; }
    public decimal ExpenseAmount { get; set; }
    public string? Notes { get; set; }
    public string? AttachmentPath { get; set; }

    public ImplementationEscortTeamMember EscortTeamMember { get; set; } = null!;
}
