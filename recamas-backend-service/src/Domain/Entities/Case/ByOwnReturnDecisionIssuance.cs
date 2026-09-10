using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.Case;

/// Έκδοση return decision εντός ByOwn case (§4.6.2.3)
public class ByOwnReturnDecisionIssuance : BaseEntity
{
    public long ByOwnCaseId { get; set; }

    public string DecisionId { get; set; } = null!;              // System-generated — string
    public string? DocumentName { get; set; }           // Free text
    public DocumentLanguage? DocumentLanguage { get; set; }
    public string? DecisionFilePath { get; set; }
    public long PreparedByUserId { get; set; }
    public DateTimeOffset? DecisionDateTime { get; set; }
    public string? Notes { get; set; }

    public ICollection<ByOwnReturnDecisionTcn> AffectedTcns { get; set; } = [];

    public ByOwnCase ByOwnCase { get; set; } = null!;
}
