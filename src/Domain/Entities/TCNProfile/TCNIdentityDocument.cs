using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;

namespace RECAMAS.Domain.Entities.TCNProfile;

/// One identity/travel document for a TCN — Implementation Specs Table 4
/// ("Identity Information"). Each document is shown as a separate entry in
/// the Overview tab and is reused by Case Management (Specs 4.4.2.3.3) as
/// the source of a case's travel documents.
public class TCNIdentityDocument : BaseEntity
{
    public long TCNProfileId { get; set; }

    public RecordSource Source { get; set; }

    public IdentityDocumentType DocumentType { get; set; }

    /// "If true, can be used in Return Case. If document type is 'Passport',
    /// then default value is set to True." (Table 4) — default applied by the
    /// Application layer on creation, not here.
    public bool IsTravelDocument { get; set; }

    public string? DocumentNumber { get; set; }

    /// Master-data country code (see TCNProfile class remarks on "Enum" fields).
    public string? IssuingCountry { get; set; }

    /// Master-data code (see TCNProfile class remarks on "Enum" fields).
    public string? IssuingAuthority { get; set; }

    public DateOnly? IssueDate { get; set; }
    public DateOnly? ExpirationDate { get; set; }

    /// Reference to a document stored in the Storage service, not the file itself.
    public Guid? AttachmentDocumentId { get; set; }
}
