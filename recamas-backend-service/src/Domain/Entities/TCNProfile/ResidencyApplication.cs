using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;

namespace RECAMAS.Domain.Entities.TCNProfile;

/// Αίτηση άδειας παραμονής (§3.3.2.2)
public class ResidencyApplication : BaseEntity
{
    public long TcnProfileId { get; set; }

    public ResidencyPermitType? TypeOfPermitRequested { get; set; } // Τύπος αιτούμενης άδειας
    public ResidencyApplicationType? TypeOfApplication { get; set; } // Initial | Renewal | Replacement
    public DateOnly? SubmissionDate { get; set; }
    public ResidenceCategory? ResidenceCategory { get; set; }
    public string? PurposeRnd { get; set; }             // RND code — free text, ARS-specific
    public DateOnly? DecisionDate { get; set; }
    public ApplicationStatus Status { get; set; }       // Pending | Approved | Rejected

    public TcnProfile TcnProfile { get; set; } = null!;
}
