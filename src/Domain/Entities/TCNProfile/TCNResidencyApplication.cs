using RECAMAS.Domain.Common;

namespace RECAMAS.Domain.Entities.TCNProfile;

/// Study Table 6 — "Each application is shown as a separate entry."
public class TCNResidencyApplication : BaseEntity
{
    public long TCNProfileId { get; set; }

    public string? TypeOfPermitRequested { get; set; }
    public string? TypeOfApplication { get; set; }
    public DateOnly? SubmissionDate { get; set; }
    public string? ResidenceCategory { get; set; }
    public string? PurposeOfResidenceRnd { get; set; }
    public DateOnly? DecisionDate { get; set; }
    public string? Status { get; set; }
}
