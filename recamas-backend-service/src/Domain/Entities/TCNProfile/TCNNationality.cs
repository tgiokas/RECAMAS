using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.TCNProfile;

/// Ένας TCN μπορεί να δηλώσει πολλαπλές εθνικότητες (§3.3.1.1)
public class TcnNationality : BaseEntity
{
    public long TcnProfileId { get; set; }              // FK → TcnProfile
    public string CountryCode { get; set; } = null!;             // ISO 3166-1 alpha-2 — string γιατί είναι standard κωδικός (πχ. "CY", "GR")
    public bool IsPrimary { get; set; }                 // Κύρια εθνικότητα για reporting
    public IdentificationStatus IdentificationStatus { get; set; } // Confirmed | Claimed | Unknown

    public TcnProfile TcnProfile { get; set; } = null!;
}
