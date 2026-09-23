using RECAMAS.Domain.Common;

namespace RECAMAS.Domain.Entities.Detention;

public class DetentionRoom : BaseEntity
{
    public long DetentionWingId { get; set; }
    public string Name { get; set; } = null!;                    // Αναγνωριστικό δωματίου — free text
    public int? Capacity { get; set; }

    public DetentionWing DetentionWing { get; set; } = null!;
}
