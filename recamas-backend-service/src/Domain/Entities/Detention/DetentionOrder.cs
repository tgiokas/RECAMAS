using RECAMAS.Domain.Common;

namespace RECAMAS.Domain.Entities.Detention;

/// LIGHT SKELETON. Real fields (order type, legal basis, alternative measures)
/// come from Specs 4.5.2.4/4.5.2.5. Re-assessment scheduling is
/// <see cref="DetentionReassessment"/>, not columns here.
public class DetentionOrder : BaseEntity
{
    public long CaseId { get; set; }

    public long? DetentionFacilityId { get; set; }

    public DateOnly? DetentionStartDate { get; set; }
    public DateOnly? DetentionEndDate { get; set; }

    /// TODO: replace with a real fixed enum once the detention status set is confirmed.
    public string? Status { get; set; }
}
