using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.Case;

/// Return Decision στο context case (§4.4.2.3.4)
public class CaseReturnDecision : BaseEntity
{
    public long CaseTcnId { get; set; }

    public string? DecisionId { get; set; }             // System-generated ID — string (human-readable reference)
    public DocumentSourceType Source { get; set; }      // RECAMAS | ARS | CASS | Other
    public ReturnDecisionIssuingAuthority IssuingAuthority { get; set; } // MD | CAS | ARS | Other
    public DateOnly? DecisionDate { get; set; }
    public DateOnly? TcnReceiptDate { get; set; }
    public string? DecisionText { get; set; }           // Κείμενο απόφασης — free text
    public DateOnly? VoluntaryReturnDeadline { get; set; }
    public int? EntryBanDurationMonths { get; set; }

    public CaseTcn CaseTcn { get; set; } = null!;
}
