using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.Case;

/// Πρόσθετος εγκριτής στο approval chain (§4.4.2.4.1)
public class AdditionalApprover : BaseEntity
{
    public long CaseId { get; set; }
    public long ApproverUserId { get; set; }
    public string? Notes { get; set; }
    public int OrderInChain { get; set; }               // Σειρά στο chain

    public ReturnCase Case { get; set; } = null!;
}
