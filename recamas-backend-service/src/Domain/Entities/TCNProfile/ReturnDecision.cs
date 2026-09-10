using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.TCNProfile;

/// Απόφαση επιστροφής TCN (§3.3.2.6)
public class ReturnDecision : BaseEntity
{
    public long TcnProfileId { get; set; }

    public ReturnDecisionIssuingAuthority IssuingAuthority { get; set; } // ARS | CASS | MD | CAS | Other
    public DateOnly? DecisionDate { get; set; }
    public string? DecisionText { get; set; }           // Κείμενο απόφασης — free text
    public DateOnly? TcnReceiptDate { get; set; }
    public DateOnly? VoluntaryReturnDeadline { get; set; }
    public int? EntryBanDurationMonths { get; set; }
    public string? DecisionFilePath { get; set; }       // Storage path

    public long? IssuingCaseId { get; set; }            // FK → ReturnCase (αν εκδόθηκε μέσω Case)

    public TcnProfile TcnProfile { get; set; } = null!;
    public ReturnCase? IssuingCase { get; set; }
}
