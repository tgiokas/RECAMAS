using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;

namespace RECAMAS.Domain.Entities.TCNProfile;

/// Specs Table 13's "Finding | Multiple" item, shaped after the equivalent
/// case-level Security Checks item (Table 36) since Table 13 itself doesn't
/// apecify the sub-fields. 
public class TCNSecurityFinding : BaseEntity
{
    public long TCNProfileId { get; set; }

    public SecurityFindingType FindingType { get; set; }
    public SeverityLevel Severity { get; set; }
    public string? Details { get; set; }
    public Guid? AttachmentDocumentId { get; set; }
}
