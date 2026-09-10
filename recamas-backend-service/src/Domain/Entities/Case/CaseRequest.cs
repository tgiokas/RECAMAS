using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.Case;

/// Αίτημα μεταξύ χρηστών εντός case (§4.3.7)
public class CaseRequest : BaseEntity
{
    public long CaseId { get; set; }

    public string RequestId { get; set; } = null!;               // System-generated human-readable ID — string
    public CaseRequestType RequestType { get; set; }    // ExpediteProcess | DocumentRequest | InformationRequest
    public long RequestedByUserId { get; set; }
    public RecipientType RecipientType { get; set; }    // Role | User
    public long? RequestedToUserId { get; set; }
    public long? RequestedToRoleId { get; set; }
    public CaseRequestStatus Status { get; set; }       // Requested | Answered | Closed

    public ICollection<CaseRequestItem> Items { get; set; } = [];

    public ReturnCase Case { get; set; } = null!;
}
