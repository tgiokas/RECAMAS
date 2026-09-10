using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.Admin;

/// In-app notification (§11)
public class Notification : BaseEntity
{
    public long RecipientUserId { get; set; }
    public long? TriggeredByUserId { get; set; }

    public NotificationEventType EventType { get; set; }
    public NotificationChannel Channel { get; set; }    // InApp | Email | Both
    public string Title { get; set; } = null!;                   // Τίτλος — free text (template-generated)
    public string? Body { get; set; }                   // Σώμα — free text

    public DeepLinkEntityType? DeepLinkEntityType { get; set; } // Case | Implementation | Profile
    public long? DeepLinkEntityId { get; set; }

    public bool IsRead { get; set; }
    public DateTimeOffset? ReadAt { get; set; }
    public bool EmailSent { get; set; }
    public DateTimeOffset? EmailSentAt { get; set; }
}
