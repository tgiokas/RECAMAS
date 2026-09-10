using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.Case;

/// Ανάθεση case σε user (§4.3.6)
public class CaseAssignment : BaseEntity
{
    public long CaseId { get; set; }
    public long UserId { get; set; }
    public long RoleId { get; set; }
    public DateTimeOffset AssignedAt { get; set; }
    public DateTimeOffset? UnassignedAt { get; set; }
    public UnassignedReason? UnassignedReason { get; set; } // Submitted | ManualUnassign

    public ReturnCase Case { get; set; } = null!;
}
