namespace RECAMAS.Domain.Entities.Case;

/// Approval item για χρηματικό κίνητρο (§4.4.2.4.2)
public class MonetaryIncentiveApprovalItem : ApprovalItem
{
    public decimal Amount { get; set; }                 // Pre-filled από Program/Country — editable
    public string? Notes { get; set; }

    public ICollection<MonetaryIncentiveApprovalTcn> AffectedTcns { get; set; } = [];
}
