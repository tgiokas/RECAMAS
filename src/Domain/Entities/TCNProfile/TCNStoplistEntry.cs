using RECAMAS.Domain.Common;

namespace RECAMAS.Domain.Entities.TCNProfile;

/// <summary>
/// Study Table 11 — "Each entry to the stoplist is shown as a separate entry."
/// EntryBanDuration/EntryBanExpirationDate are sourced from Case/Implementation
/// per the Study's own Source column, not from the Stoplist system itself —
/// kept on this entity anyway since the profile view rolls them up together
/// with the Stoplist-sourced fields.
/// </summary>
public class TCNStoplistEntry : BaseEntity
{
    public long TCNProfileId { get; set; }

    public bool StoplistHit { get; set; }
    public string? StoplistReason { get; set; }
    public string? UniqueEntryBanNumber { get; set; }
    public DateOnly? StoplistEntryDate { get; set; }

    /// <summary>Source: Case.</summary>
    public int? EntryBanDurationMonths { get; set; }

    /// <summary>Source: Implementation.</summary>
    public DateOnly? EntryBanExpirationDate { get; set; }
}
