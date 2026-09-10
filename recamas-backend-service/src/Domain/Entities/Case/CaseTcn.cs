using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.Case;

/// Junction — Case ↔ TcnProfile (M:N) με case-specific TCN data (§3.3.4.2)
public class CaseTcn : BaseEntity
{
    public long CaseId { get; set; }
    public long TcnProfileId { get; set; }

    // --- Security Checks ---
    public bool NoCriminalRecordFound { get; set; }
    public bool NoRestrictiveActivitiesFound { get; set; }
    public ICollection<SecurityFinding> SecurityFindings { get; set; } = [];

    // --- Vulnerability & Needs ---
    public bool FitToFly { get; set; }
    public string? FitToFlyAttachmentPath { get; set; } // Storage path
    public ICollection<VulnerabilityIssue> VulnerabilityIssues { get; set; } = [];

    // --- Case-level Travel Documents ---
    public ICollection<CaseTravelDocument> TravelDocuments { get; set; } = [];

    // --- Case-level Return Decision ---
    public ICollection<CaseReturnDecision> ReturnDecisions { get; set; } = [];

    // --- Pre-Return Checklist items per TCN ---
    public bool PreReturnTravelDocExists { get; set; }
    public bool PreReturnTravelDocDelivered { get; set; }
    public bool PreReturnTravelDocReceived { get; set; }
    public bool PreReturnFitToFly { get; set; }
    public bool PreReturnActivitiesCompleted { get; set; }
    public bool PreReturnNoOpenIssues { get; set; }     // Μόνο FRC

    public ReturnCase Case { get; set; } = null!;
    public TcnProfile TcnProfile { get; set; } = null!;
}
