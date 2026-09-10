using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

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
