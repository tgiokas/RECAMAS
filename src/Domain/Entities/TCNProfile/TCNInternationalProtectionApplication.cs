using RECAMAS.Domain.Common;

namespace RECAMAS.Domain.Entities.TCNProfile;

/// Study Table 8 — "Each application is shown as a separate entry."
public class TCNInternationalProtectionApplication : BaseEntity
{
    public long TCNProfileId { get; set; }

    public string? TypeOfApplication { get; set; }
    public DateOnly? SubmissionDate { get; set; }
    public DateOnly? ExpiryDate { get; set; }
    public DateOnly? DecisionDate { get; set; }
    public string? StatusDecision { get; set; }
}
