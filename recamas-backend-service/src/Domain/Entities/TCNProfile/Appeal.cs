using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.TCNProfile;

/// Έφεση σε απόφαση IP / παραμονής — από CASS (§3.3.2.5)
public class Appeal : BaseEntity
{
    public long TcnProfileId { get; set; }

    public AppealType TypeOfAppeal { get; set; }        // AdministrativeCourt | Other
    public string? AppealNumber { get; set; }           // Μοναδικός αριθμός έφεσης — free text (αριθμός πρωτοκόλλου)
    public DateOnly? AppealDate { get; set; }
    public DateOnly? DecisionDate { get; set; }
    public AppealDecisionStatus AppealStatusDecision { get; set; }

    public TcnProfile TcnProfile { get; set; } = null!;
}
