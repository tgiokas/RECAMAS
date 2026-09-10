using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.TCNProfile;

/// Τρέχον status άδειας παραμονής TCN — από ARS (§3.3.2.1)
public class ResidencyStatus : BaseEntity
{
    public long TcnProfileId { get; set; }

    public ResidencyPermitType? PermitType { get; set; }     // Τύπος άδειας παραμονής / ταξιδίου
    public DateOnly? IssueDate { get; set; }
    public ResidenceCategory? ResidenceCategory { get; set; } // Κατηγορία παραμονής
    public string? PurposeRnd { get; set; }             // Κωδικός / περιγραφή σκοπού παραμονής (RND) — free text, ARS-specific
    public DateOnly? ExpiryDate { get; set; }
    public ResidencyDocumentStatus Status { get; set; } // Active | Expired | Revoked
    public string? ResidencyDocumentNumber { get; set; } // Μοναδικός αριθμός άδειας — free text

    public TcnProfile TcnProfile { get; set; } = null!;
}
