using RECAMAS.Domain.Enums;

namespace RECAMAS.Domain.Entities.Case;

/// Approval item για escorts (§4.4.2.4.2)
public class EscortApprovalItem : ApprovalItem
{
    public EscortType EscortType { get; set; }          // AIUOfficer | MedicalProfessional | Other
    public int EscortNumber { get; set; }
    public string? Notes { get; set; }

    public ICollection<EscortExpense> EscortExpenses { get; set; } = [];
}
