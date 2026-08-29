using RECAMAS.Domain.Common;

namespace RECAMAS.Domain.Entities.Detention;

/// <summary>LIGHT SKELETON. Real fields (wings, occupancy tracking) come from Study Section 5.2.2.</summary>
public class DetentionFacility : BaseEntity
{
    public required string Name { get; set; }

    public int TotalCapacity { get; set; }
}
