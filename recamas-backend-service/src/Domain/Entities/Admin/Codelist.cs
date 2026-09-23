using RECAMAS.Domain.Common;

namespace RECAMAS.Domain.Entities.Admin;

/// Master data codelists (§8.1)
public class Codelist : BaseEntity
{
    public string ListCode { get; set; } = null!;                // Μοναδικός κωδικός — string (πχ. "COUNTRY")
    public string DisplayName { get; set; } = null!;             // String
    public string? Description { get; set; }

    public ICollection<CodelistValue> Values { get; set; } = [];
}
