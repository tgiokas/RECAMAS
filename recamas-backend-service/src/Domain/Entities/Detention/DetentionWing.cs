using RECAMAS.Domain.Common;

namespace RECAMAS.Domain.Entities.Detention;

public class DetentionWing : BaseEntity
{
    public long DetentionCenterId { get; set; }
    public string Name { get; set; } = null!;                    // Όνομα πτέρυγας — free text
    public int? Capacity { get; set; }

    public ICollection<DetentionRoom> Rooms { get; set; } = [];
    public DetentionCenter DetentionCenter { get; set; } = null!;
}
