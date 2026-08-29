using RECAMAS.Domain.Common;

namespace RECAMAS.Domain.Entities.Case;

/// <summary>
/// 1:1 detail table for CaseType.ForcedReturn. Placeholder only — real fields
/// come from Study 4.5.2 (Preliminary Detention, Orders, Alternative Measures,
/// Detention Management, Re-Assessments).
/// </summary>
public class ForcedReturnCaseDetail : BaseEntity
{
    public long CaseId { get; set; }
}
