using Cbs.Audit.Policy;
using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;

namespace RECAMAS.Domain.Entities.TCNProfile;

/// Aggregate root for the tcn_profile schema. Covers all 3 tabs of the TCN
/// Profile screen: Overview (Personal Information, Table 3; Identity
/// Information, Table 4, via <see cref="IdentityDocuments"/>), Residency/IP/
/// Decisions (Tables 5-10), Security and Arrival History (Tables 11-13), and
/// Cases and Linked Profiles (Table 14, via <see cref="Links"/> — the "Cases"
/// part of that tab, Table 15, is a read view into the Case module's own
/// Case/CaseTcnProfile entities, not modeled here).
///
/// Fields the Specs types as "Enum" but which are really admin-configurable
/// reference data (Nationality, PlaceOfBirth, IssuingCountry, IssuingAuthority,
/// etc.) are stored as plain string codes for now, not as a foreign key to a
/// master-data table — the Admin "Lists" capability (Specs Section 8.1) that
/// would own that table doesn't exist yet. Only value sets that are
/// structurally fixed by the domain itself (Gender, MdFileRelationship, the
/// identity-document Source/Type) are modeled as real C# enums.
/// [Audited]/[MaskedAudit] (Cbs.Audit.Policy — verified against the real package
/// source) are watched by the SaveChanges interceptor wired via
/// AddEntityAuditing<ApplicationDbContext>() in Program.cs, not here.
/// Type="TCNProfile" makes the auto-generated action codes
/// TCNPROFILE.CREATED/UPDATED/DELETED (AuditedAttribute.ActionPrefix defaults
/// to Type upper-cased) — matches actions.yaml. BusinessKey uses Arc even
/// though it's nullable for undocumented TCNs (no better always-populated
/// natural key exists yet; DisplayCode is not populated anywhere in the
/// codebase yet) — revisit once DisplayCode generation is built.
[Audited(Type = "TCNProfile", BusinessKey = nameof(Arc))]
public class TCNProfile : BaseEntity
{
    /// Human-facing "RECAMAS ID" shown in the UI (e.g. "TCN-00412" per the
    /// Specs's own mockup caption) — distinct from <see cref="BaseEntity.PublicId"/>,
    /// which stays the opaque identifier used across API/HTTP boundaries.
    /// Generation (sequence, format) is an Application-layer concern once
    /// TCNProfileService exists — left null until then.
    public string? DisplayCode { get; set; }

    /// Alien Registration Card number. Sourced from ARS; drives most automatic interface refreshes (Specs 9.2.1/9.3.1/9.4.1/9.5.1: "Only for TCN Profiles where ARC is available").
    public string? Arc { get; set; }

    public string? FirstNameEl { get; set; }
    public string? FirstNameEn { get; set; }
    public string? MiddleNameEl { get; set; }
    public string? MiddleNameEn { get; set; }
    public string? LastNameEl { get; set; }
    public string? LastNameEn { get; set; }

    public Gender? Gender { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    /// Master-data code, see class remarks. Sourced from ARS/Manual.
    public string? PlaceOfBirth { get; set; }

    public string? MdFileNo { get; set; }
    public MdFileRelationship? RelationshipToMdFile { get; set; }

    public string? CassFileNo { get; set; }
    public string? CassAddress { get; set; }
    public string? CassPhone { get; set; }

    public string? MdAddress { get; set; }
    public string? MdPhone { get; set; }

    ///
    /// TODO: this is a conservative starting set, not a reviewed PII policy —
    /// the team should decide the real list against Specs 12.5.7 before
    /// treating it as complete. EurodacNumber (a biometric-linked identifier)
    /// is masked in the audit trail; contact fields are not, on the view
    /// that "the address changed" is itself audit-relevant here.
    ///
    /// MaskedAudit(Mask.Full), not Cbs.Audit's own [NotAudited] — verified against
    /// the real source: NotAuditedAttribute's own doc comment says it's "for bulky
    /// or meaningless columns, not... sensitive ones which should be masked instead
    /// so that the fact of a change is still recorded", which is exactly this case.
    ///
    [MaskedAudit(Mask.Full)]
    public string? EurodacNumber { get; set; }

    public List<TCNNationality> Nationalities { get; set; } = [];
    public List<TCNIdentityDocument> IdentityDocuments { get; set; } = [];

    // --- Residency, IP, and Decisions tab (Tables 5-10) ---
    public List<TCNResidencyStatus> ResidencyStatuses { get; set; } = [];
    public List<TCNResidencyApplication> ResidencyApplications { get; set; } = [];
    public List<TCNInternationalProtectionStatus> InternationalProtectionStatuses { get; set; } = [];
    public List<TCNInternationalProtectionApplication> InternationalProtectionApplications { get; set; } = [];
    public List<TCNAppeal> Appeals { get; set; } = [];
    public List<TCNReturnDecision> ReturnDecisions { get; set; } = [];

    // --- Security and Arrival History tab (Tables 11-13) ---
    public List<TCNStoplistEntry> StoplistEntries { get; set; } = [];
    public List<TCNArrivalDeparture> ArrivalsDepartures { get; set; } = [];

    /// Table 13. Findings are <see cref="SecurityFindings"/>, not columns here.
    public bool NoCriminalRecordFound { get; set; }

    /// Table 13.
    public bool NoRestrictiveActivitiesFound { get; set; }

    public List<TCNSecurityFinding> SecurityFindings { get; set; } = [];

    // --- Cases and Linked Profiles tab (Table 14) ---
    public List<TCNProfileLink> Links { get; set; } = [];

    /// Table 3: "Age | Integer | RECAMAS | Calculated from date of birth." Never persisted.
    public int? Age => DateOfBirth is null
        ? null
        : CalculateAge(DateOfBirth.Value, DateOnly.FromDateTime(DateTime.UtcNow));

    private static int CalculateAge(DateOnly dateOfBirth, DateOnly today)
    {
        var age = today.Year - dateOfBirth.Year;
        if (dateOfBirth > today.AddYears(-age))
        {
            age--;
        }

        return age;
    }
}
