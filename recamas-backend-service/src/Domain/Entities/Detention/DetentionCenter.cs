using RECAMAS.Domain.Common;

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
