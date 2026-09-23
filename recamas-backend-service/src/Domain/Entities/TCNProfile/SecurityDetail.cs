using RECAMAS.Domain.Common;

namespace RECAMAS.Domain.Entities.TCNProfile;

/// Security check summary για TCN (§3.3.3.3)
public class SecurityDetail : BaseEntity
{
    public long TcnProfileId { get; set; }

    public bool NoCriminalRecordFound { get; set; }
    public bool NoRestrictiveActivitiesFound { get; set; }

    public ICollection<SecurityFinding> Findings { get; set; } = [];

    public TcnProfile TcnProfile { get; set; } = null!;
}
