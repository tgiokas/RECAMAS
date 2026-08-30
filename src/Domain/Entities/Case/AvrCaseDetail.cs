using RECAMAS.Domain.Common;

namespace RECAMAS.Domain.Entities.Case;

/// 1:1 detail table for CaseType.AssistedVoluntaryReturn (architecture decision:
/// shared columns live on <see cref="Case"/>, type-specific columns live here).
/// Placeholder only — real fields come from Specs 4.4.2 (Counselling
/// questionnaire, Travel Documents, Return Decision, Vulnerability/Needs,
/// Approval Items, Pre-Return checklist, Return Implementation, Re-integration),
/// a substantial module of its own.
public class AvrCaseDetail : BaseEntity
{
    public long CaseId { get; set; }
}
