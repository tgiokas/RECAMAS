using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.Case;

/// AVR counselling session — δυναμικό questionnaire sections A-H (§4.4.2.2)
public class CounsellingSession : BaseEntity
{
    public long AvrCaseId { get; set; }

    // Section A — Welcome & Language
    public string? PreferredLanguage { get; set; }      // ISO 639-1 language code — string (standard, ανοιχτό set γλωσσών)
    public EnglishProficiency EnglishProficiency { get; set; } // Fluent | Some | None
    public bool InterpreterRequired { get; set; }
    public string? InterpreterDetails { get; set; }     // Όνομα / γλώσσα / τρόπος — free text

    // Section B — Status & Current Situation
    public TcnCurrentStatus CurrentStatus { get; set; } // AsylumSeeker | Student | Worker | Other
    public string? ReturnMotivation { get; set; }       // Long text — free text
    public EntryMethod EntryMethod { get; set; }        // RegularCrossing | ThroughOccupiedArea | Other
    public string? EntryMethodDetails { get; set; }     // Free text (αν Other)
    public HowHeardAboutProgram HowHeardAboutProgram { get; set; } // Office | FRONTEX | NGO | Friend | SocialMedia | Other

    // Section C — Travel Documents
    public bool HoldsValidTravelDocument { get; set; }
    public TravelDocumentLocation? TravelDocumentLocation { get; set; } // WithApplicant | AtAsylumService | WithAuthorities | Lost | Stolen | NeverHeld
    public bool LossOrTheftReported { get; set; }
    public string? PoliceReportReference { get; set; }  // Αριθμός αναφοράς αστυνομίας — free text
    public bool HasCopyOfDocument { get; set; }

    // Section D — Legal & Financial
    public bool HasPendingFinesOrDebts { get; set; }
    public string? FinesDebtsDetails { get; set; }      // Λεπτομέρειες — free text

    // Section E — Status-specific
    public bool? HasAsylumRejectionDecision { get; set; }
    public AsylumAppealStatus? AsylumAppealStatus { get; set; } // NoAppeal | Open | Closed
    public bool? StudentVisaValid { get; set; }
    public DateOnly? StudentVisaExpiry { get; set; }
    public bool? ClosedFileWithInstitution { get; set; }
    public bool? HoldsPinkSlip { get; set; }
    public DateOnly? PinkSlipExpiry { get; set; }
    public bool? HasEmployerReleasePaper { get; set; }
    public string? OtherLegalSituation { get; set; }   // Free text (αν B1 = Other)

    // Section F — Family
    public TravelGroupType TravelGroup { get; set; }    // Alone | WithSpouse | WithChildren | WithSpouseAndChildren | SingleParentWithChild
    public bool? IsMarried { get; set; }
    public string? OtherParentLocation { get; set; }   // Τοποθεσία άλλου γονέα — free text
    public bool? HasFamilyInCyprus { get; set; }
    public string? FamilyInCyprusDetails { get; set; } // Free text
    public bool? FamilyPreviouslyReturned { get; set; }
    public string? FamilyReturnDetails { get; set; }   // Free text

    // Section G — Vulnerability
    public VulnerabilityType[]? VulnerabilityTypes { get; set; } // [Flags] enum — None/Pregnancy/Medical/Disability/UnaccompaniedMinor/Elderly/SingleParent/TraffickingVictim/MentalHealth/Other
    public bool NeedsAccommodation { get; set; }
    public string? AccommodationDetails { get; set; }  // Free text
    public bool HasMedicalCondition { get; set; }
    public string? MedicalDetails { get; set; }        // Confidential — free text
    public bool NeedsAirportAssistance { get; set; }
    public string? AirportAssistanceDetails { get; set; } // Free text

    // Section H — Programme Choice
    public AvrProgram? PreferredProgram { get; set; }  // AVRCyprus | EURP | Undecided
    public ProceedWithReturn WishesToProceed { get; set; } // Yes | No | StillConsidering
    public string? ContactPhone { get; set; }          // Τηλέφωνο — free text
    public string? CurrentAddress { get; set; }        // Διεύθυνση — free text
    public string? PreferredAirport { get; set; }      // IATA code ή free text (best effort)

    public ICollection<CounsellingChild> AccompanyingChildren { get; set; } = [];

    public AvrCase AvrCase { get; set; } = null!;
}
