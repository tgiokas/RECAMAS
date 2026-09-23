namespace RECAMAS.Domain.Entities.Case;

/// Approval item για entry ban (§4.4.2.4.2 / §4.5.2.4.3)
public class EntryBanApprovalItem : ApprovalItem
{
    public int EntryBanDurationMonths { get; set; }
    public string? Notes { get; set; }
}
