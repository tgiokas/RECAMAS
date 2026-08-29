using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;

namespace RECAMAS.Domain.Entities.TCNProfile;

/// <summary>
/// One identity/travel document for a TCN — Implementation Study Table 4
/// ("Identity Information"). Each document is shown as a separate entry in
/// the Overview tab and is reused by Case Management (Study 4.4.2.3.3) as
/// the source of a case's travel documents.
/// </summary>
public class TCNIdentityDocument : BaseEntity
{
    public long TCNProfileId { get; set; }

    public RecordSource Source { get; set; }

    public IdentityDocumentType DocumentType { get; set; }

    /// <summary>
    /// "If true, can be used in Return Case. If document type is 'Passport',
    /// then default value is set to True." (Table 4) — default applied by the
    /// Application layer on creation, not here.
    /// </summary>
    public bool IsTravelDocument { get; set; }

    public string? DocumentNumber { get; set; }

    /// <summary>Master-data country code (see TCNProfile class remarks on "Enum" fields).</summary>
    public string? IssuingCountry { get; set; }

    /// <summary>Master-data code (see TCNProfile class remarks on "Enum" fields).</summary>
    public string? IssuingAuthority { get; set; }

    public DateOnly? IssueDate { get; set; }
    public DateOnly? ExpirationDate { get; set; }

    /// <summary>Reference to a document stored in the Storage service, not the file itself.</summary>
    public Guid? AttachmentDocumentId { get; set; }
}
