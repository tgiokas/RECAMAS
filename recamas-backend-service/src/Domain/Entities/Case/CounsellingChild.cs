using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.Case;

/// Συνοδευόμενο παιδί στο counselling session (§4.4.2.2 Section F)
public class CounsellingChild : BaseEntity
{
    public long CounsellingSessionId { get; set; }

    public bool HasValidTravelDocuments { get; set; }
    public string? PlaceOfBirth { get; set; }           // Χώρα / πόλη γέννησης — free text
    public bool HasBirthCertificate { get; set; }
    public bool HasConsentToTravel { get; set; }

    public CounsellingSession CounsellingSession { get; set; } = null!;
}
