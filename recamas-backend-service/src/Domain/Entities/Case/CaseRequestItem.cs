using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.Case;

/// Μήνυμα (request ή reply) σε CaseRequest thread (§4.3.7)
public class CaseRequestItem : BaseEntity
{
    public long CaseRequestId { get; set; }

    public bool IsReply { get; set; }                   // False = initial, True = reply
    public long AuthorUserId { get; set; }
    public DateTimeOffset ItemDateTime { get; set; }
    public string? Notes { get; set; }                  // Κείμενο — free text
    public string? AttachmentPath { get; set; }

    public CaseRequest CaseRequest { get; set; } = null!;
}
