using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.TCNProfile;

/// Ταυτοποιητικά έγγραφα TCN — κάθε document ξεχωριστή εγγραφή (§3.3.1.2)
public class IdentityDocument : BaseEntity
{
    public long TcnProfileId { get; set; }

    public DocumentSourceType Source { get; set; }      // ARS | CASS | RECAMAS | Case
    public DocumentType DocumentType { get; set; }      // Passport | CountryIssuedId | Other
    public bool IsTravelDocument { get; set; }          // True → μπορεί να χρησιμοποιηθεί σε Return Case
    public string? DocumentNumber { get; set; }         // Αριθμός εγγράφου — free text (διαφορετική μορφή ανά χώρα)
    public string? IssuingCountryCode { get; set; }     // ISO 3166-1 alpha-2 — string (standard country code)
    public string? IssuingAuthority { get; set; }       // Αρχή έκδοσης — free text (ανοιχτό set ανά χώρα)
    public DateOnly? IssueDate { get; set; }
    public DateOnly? ExpirationDate { get; set; }
    public string? AttachmentPath { get; set; }         // Storage path του scan

    public TcnProfile TcnProfile { get; set; } = null!;
}
