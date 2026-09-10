using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.TCNProfile;

/// Κεντρική οντότητα — αποθηκεύει όλη την πληροφορία για έναν TCN (Third-Country National)
/// §3.1 — "The TCN profile is the central entity holding all information about a TCN"
public class TcnProfile : BaseEntity
{
    public string RecamasId { get; set; } = null!;               // Μοναδικό system-generated ID (πχ. TCN-2026-00001)
    public string? Arc { get; set; }                    // Alien Registration Card number — από ARS interface
    public string? EurodacNumber { get; set; }          // EURODAC biometric reference — από CASS

    // --- Ονόματα (bilingual EL/EN) ---
    public string? FirstNameEl { get; set; }            // Όνομα στα Ελληνικά — από ARS/Manual
    public string? FirstNameEn { get; set; }            // Όνομα στα Αγγλικά — από ARS/Manual
    public string? MiddleNameEl { get; set; }           // Μεσαίο όνομα EL
    public string? MiddleNameEn { get; set; }           // Μεσαίο όνομα EN
    public string? LastNameEl { get; set; }             // Επώνυμο EL
    public string? LastNameEn { get; set; }             // Επώνυμο EN

    // --- Δημογραφικά ---
    public GenderType Gender { get; set; }              // M / F / U (Unknown) — από ARS
    public DateOnly? DateOfBirth { get; set; }          // Ημερομηνία γέννησης — από ARS/Manual
    public string? PlaceOfBirth { get; set; }           // Τόπος γέννησης — free text (πόλη ή περιοχή, όχι enum: αχανές set)
    // Age: computed property = DateTime.Today.Year - DateOfBirth.Year (§3.3.1.1)

    // --- Αναφορές σε εξωτερικά συστήματα ---
    public string? MdFileNo { get; set; }               // Migration Department file number — από ARS
    public string? ArsFolderId { get; set; }             // [ΠΡΟΣΤΕΘΗΚΕ] Βάση για πρόταση linked-profile — έλειπε, βλ. tcn_profile.ars_folder_id
                                                           // PDF ref: §2.2.4 "Linked Profile Suggestion" — "Based on the ARS Folder Number, RECAMAS
                                                           // performs an interface call to ARS and retrieves any TCN Profiles that have the same ARS Folder Number"
    public MdFileRelationship? MdFileRelationship { get; set; } // Ρόλος στο MD file: Principal / MainDependant / Dependant
    public string? CassFileNo { get; set; }             // Cyprus Asylum Service file number — από CASS
    public string? CassAddress { get; set; }            // Διεύθυνση από CASS (read-only, free text)
    public string? MdAddress { get; set; }              // Διεύθυνση από ARS (read-only, free text)
    public string? CassPhone { get; set; }              // Τηλέφωνο από CASS (read-only)
    public string? MdPhone { get; set; }                // Τηλέφωνο από ARS (read-only)

    // --- Βιομετρικά (§12.1.1) ---
    public string? PhotographStoragePath { get; set; }  // Path στο storage (ISO/IEC 19794-5:2011)
    public string? FingerprintNistPath { get; set; }    // Path NIST ITL 1-2011 file
    public float[]? FingerprintVector { get; set; }     // pgvector embedding για biometric search

    // --- Status & Flags (υπολογίζονται από BRE / workflow) ---
    public TcnProfileStatus Status { get; set; }        // §3.4: AVRApplicationPending, AVRReturnPending, Departed, κλπ
    public bool FlagSecurityIssues { get; set; }        // §3.5: Υπάρχει τουλάχιστον ένα security issue item
    public bool FlagMinor { get; set; }                 // §3.5: Ηλικία < 18
    public bool FlagNoArc { get; set; }                 // §3.5: Δεν υπάρχει ARC
    public bool FlagNoTravelDocument { get; set; }      // §3.5: Δεν υπάρχει travel document για επιστροφή

    // --- Anonymization (§12.1.1, §12.5.12) ---
    public bool IsAnonymized { get; set; }              // Έχει γίνει anonymization βάσει retention policy
    public DateTimeOffset? AnonymizedAt { get; set; }   // Πότε έγινε anonymization
    public long? AnonymizedByUserId { get; set; }       // Ποιος έκανε anonymization

    // --- Πηγή δεδομένων ---
    public DataSourceType PrimarySource { get; set; }   // ARS | CASS | Manual — από που δημιουργήθηκε το profile

    // --- Relationships ---
    public ICollection<TcnNationality> Nationalities { get; set; } = [];
    public ICollection<IdentityDocument> IdentityDocuments { get; set; } = [];
    public ICollection<ResidencyStatus> ResidencyStatuses { get; set; } = [];
    public ICollection<ResidencyApplication> ResidencyApplications { get; set; } = [];
    public ICollection<IpStatus> IpStatuses { get; set; } = [];
    public ICollection<IpApplication> IpApplications { get; set; } = [];
    public ICollection<Appeal> Appeals { get; set; } = [];
    public ICollection<ReturnDecision> ReturnDecisions { get; set; } = [];
    public ICollection<StoplistEntry> StoplistEntries { get; set; } = [];
    public ICollection<ArrivalDeparture> ArrivalDepartures { get; set; } = [];
    public ICollection<SecurityDetail> SecurityDetails { get; set; } = [];
    public ICollection<LinkedProfile> LinkedProfilesFrom { get; set; } = [];
    public ICollection<LinkedProfile> LinkedProfilesTo { get; set; } = [];
    public ICollection<CaseTcn> CaseTcns { get; set; } = [];
}
