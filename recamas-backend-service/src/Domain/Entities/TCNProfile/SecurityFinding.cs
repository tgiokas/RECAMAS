using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.TCNProfile;

/// Μεμονωμένο εύρημα ασφαλείας (§4.4.2.3.2)
public class SecurityFinding : BaseEntity
{
    public long SecurityDetailId { get; set; }
    public long? CaseTcnId { get; set; }               // FK → CaseTcn (αν εντοπίστηκε μέσα σε case)

    public FindingType FindingType { get; set; }        // CriminalRecord | RestrictiveActivity | Other
    public SeverityLevel SeverityLevel { get; set; }   // Low | Medium | High
    public string? Details { get; set; }                // Ελεύθερο κείμενο περιγραφής — free text
    public string? AttachmentPath { get; set; }         // Storage path

    public SecurityDetail SecurityDetail { get; set; } = null!;
}
