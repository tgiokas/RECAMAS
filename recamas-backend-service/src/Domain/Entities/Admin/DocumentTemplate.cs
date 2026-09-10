using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.Admin;

/// Template εγγράφου (§8.2)
public class DocumentTemplate : BaseEntity
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public CaseType CaseType { get; set; }
    public DocumentLanguage? Language { get; set; }     // Greek | English | Other
    public bool IsActive { get; set; }
    public int Version { get; set; }
    public string TemplateFilePath { get; set; } = null!;        // Storage path
    public string? PlaceholdersDefinition { get; set; } // JSON — string (structured data)

    public long? PreviousVersionId { get; set; }
    public ICollection<DocumentTemplate> PreviousVersions { get; set; } = [];
}
