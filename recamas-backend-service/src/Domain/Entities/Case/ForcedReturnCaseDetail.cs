using RECAMAS.Domain.Common;

namespace RECAMAS.Domain.Entities.Case;

/// 1:1 detail table for CaseType.ForcedReturn. 
/// real fields come from Specs 4.5.2 (Preliminary Detention, Orders, Alternative Measures,
/// Detention Management, Re-Assessments).
public class ForcedReturnCaseDetail : BaseEntity
{
    public long CaseId { get; set; }
}
