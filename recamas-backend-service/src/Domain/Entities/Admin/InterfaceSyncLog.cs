using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.Admin;

/// Log εκτέλεσης interface sync (§9, §12.3)
public class InterfaceSyncLog : BaseEntity
{
    public ExternalSystem ExternalSystem { get; set; }
    public SyncTriggerType TriggerType { get; set; }
    public long? TriggeredByUserId { get; set; }
    public long? RelatedTcnProfileId { get; set; }
    public long? RelatedCaseId { get; set; }

    public DateTimeOffset StartedAt { get; set; }
    public DateTimeOffset? CompletedAt { get; set; }
    public SyncStatus Status { get; set; }
    public string? ErrorMessage { get; set; }           // Free text (technical message)
    public int? RetryCount { get; set; }
}
