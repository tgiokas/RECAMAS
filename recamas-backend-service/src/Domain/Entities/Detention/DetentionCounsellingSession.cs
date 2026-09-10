using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.Detention;

/// Συνεδρία συμβουλευτικής κατά κράτηση (§4.5.2.5.4)
public class DetentionCounsellingSession : BaseEntity
{
    public long ForcedReturnCaseId { get; set; }

    public string SessionId { get; set; } = null!;               // System-generated — string
    public DateTimeOffset SessionDateTime { get; set; }
    public DetentionCounsellorType CounsellorType { get; set; } // AIUOfficer | FRONTEX | Other
    public long? CounsellorUserId { get; set; }
    public string? Notes { get; set; }
    public string? AttachmentPath { get; set; }
    public CounsellingSessionStatus Status { get; set; } // InProgress | Completed

    public ForcedReturnCase ForcedReturnCase { get; set; } = null!;
}
