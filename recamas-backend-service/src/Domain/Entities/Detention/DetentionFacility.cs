using RECAMAS.Domain.Common;

namespace RECAMAS.Domain.Entities.Detention;

/// LIGHT SKELETON. Real fields (wings, occupancy tracking) come from Specs Section 5.2.2.
public class DetentionFacility : BaseEntity
{
    public required string Name { get; set; }

    public int TotalCapacity { get; set; }
}
