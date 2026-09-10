using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.Case;

/// Alternative Measure για FRC (§4.5.2.4.2)
public class AlternativeMeasureApprovalItem : ApprovalItem
{
    public AlternativeMeasureType MeasureType { get; set; }
    public string? Notes { get; set; }
    public string? AttachmentPath { get; set; }
}
