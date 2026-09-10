using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.Case;

/// Uploaded ή generated έγγραφο εντός case (§4.4.2.9)
public class CaseDocument : BaseEntity
{
    public long CaseId { get; set; }
    public long? TcnProfileId { get; set; }             // Null αν αφορά το case συνολικά

    public CaseDocumentType DocumentType { get; set; }
    public CaseDocumentKind Kind { get; set; }          // Generated | Uploaded
    public string? Description { get; set; }            // Free text
    public long? UploadedByUserId { get; set; }
    public DateTimeOffset? UploadDateTime { get; set; }
    public string AttachmentPath { get; set; } = null!;          // Storage path — UUID filename (§12.5.16)
    public string? OriginalFilename { get; set; }       // Αρχικό όνομα πριν rename
    public string? MimeType { get; set; }               // MIME type για validation — string (standard IANA)

    // QES signing (§9.6 / §12.1.2)
    public bool IsQesSigned { get; set; }
    public bool? QesVerified { get; set; }
    public DateTimeOffset? QesSignedAt { get; set; }
    public long? QesSignedByUserId { get; set; }
    public string? JccTransactionId { get; set; }       // JCC reference — free text (external system format)

    public ReturnCase Case { get; set; } = null!;
    public TcnProfile? TcnProfile { get; set; }
}
