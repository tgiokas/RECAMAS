using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.Detention;

/// Κέντρο κράτησης (§5.2.2)
public class DetentionCenter : BaseEntity
{
    public string Name { get; set; } = null!;                    // Επωνυμία — free text
    public string? Location { get; set; }               // Τοποθεσία — free text
    public int TotalCapacity { get; set; }
    public bool IsActive { get; set; }

    public ICollection<DetentionWing> Wings { get; set; } = [];
    public ICollection<DetentionRecord> DetentionRecords { get; set; } = [];
}
