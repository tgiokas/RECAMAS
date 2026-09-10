using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.Case;

/// Διαδικασία έκδοσης travel document εντός case (§4.4.2.6)
public class TravelDocumentIssuance : BaseEntity
{
    public long CaseId { get; set; }
    public long TcnProfileId { get; set; }

    public string IssuanceId { get; set; } = null!;              // System-generated — string
    public DateOnly? RequestDate { get; set; }
    public TravelDocumentType DocumentType { get; set; }
    public string? Notes { get; set; }
    public long RequestedByUserId { get; set; }
    public RequestingAuthority? RequestingAuthority { get; set; } // MigrationDepartment | AIU | Other
    public string? IssuingCountryCode { get; set; }     // ISO 3166-1 alpha-2 — string
    public string? IssuingAuthority { get; set; }       // Αρχή έκδοσης — free text (ανά χώρα διαφέρει)
    public DateOnly? IssueDate { get; set; }
    public TravelDocIssuanceStatus Status { get; set; } // Requested | Issued | Rejected

    // Issued document details
    public string? IssuedDocumentNumber { get; set; }   // Free text
    public DateOnly? IssuedDocumentExpirationDate { get; set; }
    public string? IssuedDocumentAttachmentPath { get; set; }

    public ReturnCase Case { get; set; } = null!;
    public TcnProfile TcnProfile { get; set; } = null!;
}
