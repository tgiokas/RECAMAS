using Shared.Domain.Common;
using Shared.Domain.Enums;

namespace ArsApi.Domain.Entities;

/// <summary>
/// Represents a full ARS record as stored in the ars_records table.
/// Mirrors the Request/Response fields defined in the Implementation Study §9.2.
/// </summary>
public class ArsRecord : BaseEntity
{
    // === IDENTIFICATION (used in search request) ===
    public string Arc { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public Nationality Nationality { get; set; }
    public Gender Gender { get; set; }
    public string? PassportNo { get; set; }
    public DateTime? PassportExpirationDate { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? PlaceOfBirth { get; set; }
    public string? Address { get; set; }
    public string? PhoneNo { get; set; }
    public string? PhotographPath { get; set; }
    public string? MdFileNo { get; set; }
    public RelationshipToFile? RelationshipToFile { get; set; }

    /// <summary>
    /// ARS folder number grouping records that belong to the same case file
    /// (e.g. family members submitted/registered together). See RECAMAS
    /// §2.2.4 "Linked Profile Suggestion".
    /// </summary>
    public string? ArsFolderNumber { get; set; }

    // === RESIDENCY STATUS ===
    public PermitType? PermitType { get; set; }
    public DateTime? ResidencyIssueDate { get; set; }
    public ResidenceCategory? ResidenceCategory { get; set; }
    public string? PurposeOfResidence { get; set; }
    public DateTime? ResidencyExpiryDate { get; set; }
    public ResidencyStatus? ResidencyStatus { get; set; }
    public string? ResidencyDocumentNumber { get; set; }

    // === RESIDENCY APPLICATION ===
    public PermitType? TypeOfPermitRequested { get; set; }
    public string? TypeOfApplication { get; set; }
    public DateTime? ApplicationSubmissionDate { get; set; }
    public ResidenceCategory? ApplicationResidenceCategory { get; set; }
    public string? ApplicationPurposeOfResidence { get; set; }
    public DateTime? ApplicationDecisionDate { get; set; }
    public ResidencyStatus? ApplicationStatus { get; set; }

    // === RETURN DECISION ===
    public DateTime? ReturnDecisionDate { get; set; }
    public string? ReturnDecisionText { get; set; }
    public int? VoluntaryReturnDeadlineDays { get; set; }
    public int? EntryBanDurationMonths { get; set; }
}
