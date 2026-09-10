using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.Case;

/// Return Decision ως approval item (§4.4.2.4.1)
public class ReturnDecisionApprovalItem : ApprovalItem
{
    public string? DecisionId { get; set; }             // System-generated — string
    public string? DocumentName { get; set; }           // Τίτλος εγγράφου — free text
    public DocumentLanguage? DocumentLanguage { get; set; } // Greek | English | Other
    public string? DecisionFilePath { get; set; }
    public long? PreparedByUserId { get; set; }          // [ΠΡΟΣΤΕΘΗΚΕ] Ξεχωριστό από ApprovalItem.AssessedByUserId (=approved by)
                                                           // — έλειπε η διάκριση "συνέταξε" vs "ενέκρινε", βλ. case_return_decision.prepared_by
                                                           // PDF ref: ΟΧΙ άμεσο match — το Table 40 "AVR Return Decisions" (§4.4.2.4.1) έχει μόνο "Approved by".
                                                           // Αναλογία με Table 91 "BOR Return Decision Issuance" (§4.6.2.3), που έχει ρητό πεδίο "Prepared by"
                                                           // στην κλάση ByOwnReturnDecisionIssuance — χρειάζεται επιβεβαίωση αν ισχύει και εδώ
    public DateTimeOffset? DecisionDateTime { get; set; }
    public DateOnly? VoluntaryDepartureDeadline { get; set; }

    public ICollection<ReturnDecisionApprovalTcn> AffectedTcns { get; set; } = [];
}
