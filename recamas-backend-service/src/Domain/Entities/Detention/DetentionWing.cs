using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.Detention;

public class DetentionWing : BaseEntity
{
    public long DetentionCenterId { get; set; }
    public string Name { get; set; } = null!;                    // Όνομα πτέρυγας — free text
    public int? Capacity { get; set; }

    public ICollection<DetentionRoom> Rooms { get; set; } = [];
    public DetentionCenter DetentionCenter { get; set; } = null!;
}
