using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;

namespace RECAMAS.Domain.Entities.TCNProfile;

/// <summary>
/// Aggregate root for the tcn_profile schema. Covers the "Overview" tab of the
/// TCN Profile screen — Personal Information (Implementation Study Table 3) and
/// Identity Information (Table 4, via <see cref="IdentityDocuments"/>).
///
/// Deliberately excluded from this pass: Photograph and Fingerprints (Table 3).
/// Both are optional (Req.=No) file attachments; fingerprints in particular
/// belong to the broader ANSI/NIST-ITL biometric capability (Study 12.1.1),
/// which needs its own design pass and is tracked as a separate open item —
/// not modeled here at all.
///
/// Fields the Study types as "Enum" but which are really admin-configurable
/// reference data (Nationality, PlaceOfBirth, IssuingCountry, IssuingAuthority,
/// etc.) are stored as plain string codes for now, not as a foreign key to a
/// master-data table — the Admin "Lists" capability (Study Section 8.1) that
/// would own that table doesn't exist yet. Only value sets that are
/// structurally fixed by the domain itself (Gender, MdFileRelationship, the
/// identity-document Source/Type) are modeled as real C# enums.
/// </summary>
public class TCNProfile : BaseEntity
{
    /// <summary>
    /// Human-facing "RECAMAS ID" shown in the UI (e.g. "TCN-00412" per the
    /// Study's own mockup caption) — distinct from <see cref="BaseEntity.PublicId"/>,
    /// which stays the opaque identifier used across API/HTTP boundaries.
    /// Generation (sequence, format) is an Application-layer concern once
    /// TCNProfileService exists — left null until then.
    /// </summary>
    public string? DisplayCode { get; set; }

    /// <summary>Alien Registration Card number. Sourced from ARS; drives most automatic interface refreshes (Study 9.2.1/9.3.1/9.4.1/9.5.1: "Only for TCN Profiles where ARC is available").</summary>
    public string? Arc { get; set; }

    public string? FirstNameEl { get; set; }
    public string? FirstNameEn { get; set; }
    public string? MiddleNameEl { get; set; }
    public string? MiddleNameEn { get; set; }
    public string? LastNameEl { get; set; }
    public string? LastNameEn { get; set; }

    public Gender? Gender { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    /// <summary>Master-data code, see class remarks. Sourced from ARS/Manual.</summary>
    public string? PlaceOfBirth { get; set; }

    public string? MdFileNo { get; set; }
    public MdFileRelationship? RelationshipToMdFile { get; set; }

    public string? CassFileNo { get; set; }
    public string? CassAddress { get; set; }
    public string? CassPhone { get; set; }

    public string? MdAddress { get; set; }
    public string? MdPhone { get; set; }

    public string? EurodacNumber { get; set; }

    public List<TCNNationality> Nationalities { get; set; } = [];
    public List<TCNIdentityDocument> IdentityDocuments { get; set; } = [];

    /// <summary>Table 3: "Age | Integer | RECAMAS | Calculated from date of birth." Never persisted.</summary>
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
