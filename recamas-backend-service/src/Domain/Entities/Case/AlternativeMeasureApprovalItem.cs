using RECAMAS.Domain.Enums;

namespace RECAMAS.Domain.Entities.Case;

/// Alternative Measure για FRC (§4.5.2.4.2)
public class AlternativeMeasureApprovalItem : ApprovalItem
{
    public AlternativeMeasureType MeasureType { get; set; }
    public string? Notes { get; set; }
    public string? AttachmentPath { get; set; }
}
