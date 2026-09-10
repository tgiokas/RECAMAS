using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.ReturnImplementation;

/// Μέλος escort team (§6.3.1.3.1.2)
public class ImplementationEscortTeamMember : BaseEntity
{
    public long ReturnImplementationId { get; set; }

    public EscortType EscortType { get; set; }
    public long? EscortUserId { get; set; }             // Αναγνωριστικό χρήστη από το εξωτερικό identity system
    public string? ExternalEscortName { get; set; }     // Free text (αν εξωτερικός)

    public ICollection<ImplementationEscortExpense> EscortExpenses { get; set; } = [];

    public ReturnImplementation ReturnImplementation { get; set; } = null!;
}
