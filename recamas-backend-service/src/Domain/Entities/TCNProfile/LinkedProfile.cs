using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.TCNProfile;

/// Σύνδεση δύο TCN profiles (§3.3.4.1)
public class LinkedProfile : BaseEntity
{
    public long FromTcnProfileId { get; set; }
    public long ToTcnProfileId { get; set; }
    public LinkedProfileRelationship Relationship { get; set; } // Spouse | Child | Parent | Sibling | DuplicateMerged | Other
    public string? Notes { get; set; }                  // Σημειώσεις — free text

    public TcnProfile FromTcnProfile { get; set; } = null!;
    public TcnProfile ToTcnProfile { get; set; } = null!;
}
