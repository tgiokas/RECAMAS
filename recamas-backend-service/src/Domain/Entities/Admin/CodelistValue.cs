using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.Admin;

/// Τιμή codelist — απενεργοποιείται αντί διαγραφής (§8.1)
public class CodelistValue : BaseEntity
{
    public long CodelistId { get; set; }

    public string Code { get; set; } = null!;                    // Μοναδικός κωδικός τιμής — string
    public string DisplayNameEl { get; set; } = null!;
    public string DisplayNameEn { get; set; } = null!;
    public bool IsActive { get; set; }
    public int? SortOrder { get; set; }

    public Codelist Codelist { get; set; } = null!;
}
