using RECAMAS.Domain.Common;

namespace RECAMAS.Domain.Entities.Case;

/// <summary>
/// 1:1 detail table for CaseType.VoluntaryReturnOwnMeans. Placeholder only —
/// real fields come from Study 4.6.2 (Return Decision linkage, Agreement/Sign-Off,
/// Departure Confirmation) — the lightest of the 3 workflows.
/// </summary>
public class VoluntaryReturnOwnMeansCaseDetail : BaseEntity
{
    public long CaseId { get; set; }
}
