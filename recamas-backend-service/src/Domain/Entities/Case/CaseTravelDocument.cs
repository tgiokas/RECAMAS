using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.Case;

/// Travel document εντός case — από profile ή νέο (§4.4.2.3.3)
public class CaseTravelDocument : BaseEntity
{
    public long CaseTcnId { get; set; }

    public DocumentType DocumentType { get; set; }
    public DocumentSourceType Source { get; set; }
    public string? DocumentNumber { get; set; }         // Αριθμός εγγράφου — free text
    public string? IssuingCountryCode { get; set; }     // ISO 3166-1 alpha-2 — string (standard code)
    public string? IssuingAuthority { get; set; }       // Αρχή έκδοσης — free text (ανοιχτό set)
    public DateOnly? ExpirationDate { get; set; }
    public bool IssuedForReturnCase { get; set; }       // True αν από Travel Doc Issuance process
    public bool CanBeUsedForReturn { get; set; }
    public TravelDocumentPhysicalLocation? PhysicalLocation { get; set; } // WithApplicant | AtAsylumService | WithAuthorities | Lost | Stolen | NeverHeld | AIUOffice
    public bool Delivered { get; set; }
    public string? AttachmentPath { get; set; }

    public long? SourceIdentityDocumentId { get; set; } // FK → IdentityDocument (αν από profile)

    public CaseTcn CaseTcn { get; set; } = null!;
}
