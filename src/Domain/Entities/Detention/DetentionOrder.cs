using RECAMAS.Domain.Common;

namespace RECAMAS.Domain.Entities.Detention;

/// <summary>
/// LIGHT SKELETON. Real fields (order type, legal basis, alternative measures)
/// come from Study 4.5.2.4/4.5.2.5. Re-assessment scheduling is
/// <see cref="DetentionReassessment"/>, not columns here.
/// </summary>
public class DetentionOrder : BaseEntity
{
    public long CaseId { get; set; }

    public long? DetentionFacilityId { get; set; }

    public DateOnly? DetentionStartDate { get; set; }
    public DateOnly? DetentionEndDate { get; set; }

    /// <summary>TODO: replace with a real fixed enum once the detention status set is confirmed.</summary>
    public string? Status { get; set; }
}
