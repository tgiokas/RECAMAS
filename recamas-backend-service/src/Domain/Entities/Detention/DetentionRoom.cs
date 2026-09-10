using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.Detention;

public class DetentionRoom : BaseEntity
{
    public long DetentionWingId { get; set; }
    public string Name { get; set; } = null!;                    // Αναγνωριστικό δωματίου — free text
    public int? Capacity { get; set; }

    public DetentionWing DetentionWing { get; set; } = null!;
}
