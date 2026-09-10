using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.ReturnImplementation;

/// Uploaded έγγραφο implementation (§6.3.1.5)
public class ImplementationDocument : BaseEntity
{
    public long ReturnImplementationId { get; set; }

    public ImplementationDocumentType DocumentType { get; set; }
    public string? Description { get; set; }
    public long UploadedByUserId { get; set; }
    public DateTimeOffset UploadDateTime { get; set; }
    public string AttachmentPath { get; set; } = null!;          // UUID filename
    public string? MimeType { get; set; }               // IANA MIME type — string (standard)

    public ReturnImplementation ReturnImplementation { get; set; } = null!;
}
