using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.Case;

/// Notes επιστροφής για διορθώσεις (§4.4.2.4.2)
public class AdjustmentNote : BaseEntity
{
    public long CaseId { get; set; }

    public string Notes { get; set; } = null!;                   // Λόγος — free text
    public DateTimeOffset SubmittedForAdjustmentAt { get; set; }

    public ReturnCase Case { get; set; } = null!;
}
