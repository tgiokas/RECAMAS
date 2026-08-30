using RECAMAS.Domain.Common;

namespace RECAMAS.Domain.Entities.Case;

/// 1:1 detail table for CaseType.VoluntaryReturnOwnMeans. Placeholder only —
/// real fields come from Specs 4.6.2 (Return Decision linkage, Agreement/Sign-Off,
/// Departure Confirmation) — the lightest of the 3 workflows.
public class VoluntaryReturnOwnMeansCaseDetail : BaseEntity
{
    public long CaseId { get; set; }
}
