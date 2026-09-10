using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.Admin;

/// Master data codelists (§8.1)
public class Codelist : BaseEntity
{
    public string ListCode { get; set; } = null!;                // Μοναδικός κωδικός — string (πχ. "COUNTRY")
    public string DisplayName { get; set; } = null!;             // String
    public string? Description { get; set; }

    public ICollection<CodelistValue> Values { get; set; } = [];
}
