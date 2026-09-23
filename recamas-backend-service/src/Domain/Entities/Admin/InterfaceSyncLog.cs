using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;

namespace RECAMAS.Domain.Entities.Admin;

/// Log εκτέλεσης interface sync (§9, §12.3)
public class InterfaceSyncLog : BaseEntity
{
    public ExternalSystem ExternalSystem { get; set; }
    public string Operation { get; set; } = null!;
    public string CorrelationId { get; set; } = null!;
    public SyncTriggerType TriggerType { get; set; }
    public long? TriggeredByUserId { get; set; }
    public long? RelatedTcnProfileId { get; set; }
    public long? RelatedCaseId { get; set; }

    public DateTimeOffset StartedAt { get; set; }
    public DateTimeOffset? CompletedAt { get; set; }
    public SyncStatus Status { get; set; }
    public int? HttpStatusCode { get; set; }
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }           // Free text (technical message)
    public int? RetryCount { get; set; }
    public long? DurationMilliseconds { get; set; }
    public string? ExternalReference { get; set; }
    public string? RequestPayloadHash { get; set; }
    public string? ResponsePayloadHash { get; set; }
}
