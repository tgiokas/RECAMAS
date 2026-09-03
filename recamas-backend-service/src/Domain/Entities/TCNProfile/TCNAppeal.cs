using RECAMAS.Domain.Common;

namespace RECAMAS.Domain.Entities.TCNProfile;

/// Specs Table 9 — "Each appeal is shown as a separate entry." Appeal type is to the Administrative Court of International Protection (ΔΔΔΠ).
public class TCNAppeal : BaseEntity
{
    public long TCNProfileId { get; set; }

    public string? TypeOfAppeal { get; set; }
    public string? AppealNumber { get; set; }
    public DateOnly? AppealDate { get; set; }
    public DateOnly? DecisionDate { get; set; }
    public string? AppealStatusDecision { get; set; }
}
