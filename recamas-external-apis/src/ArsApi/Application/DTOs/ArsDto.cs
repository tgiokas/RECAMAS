using Shared.Domain.Enums;

namespace ArsApi.Application.DTOs;

/// <summary>Search request — Table 151 ARS Request Fields</summary>
public record ArsSearchRequest(
    string? Arc,
    string? Name,
    string? Surname,
    Nationality? Nationality,
    string? PassportNo,
    DateTime? DateOfBirth,
    string? MdFileNumber
);

/// <summary>Create/upsert request — all fields writable</summary>
public record ArsCreateRequest(
    string Arc,
    string FirstName,
    string LastName,
    Nationality Nationality,
    Gender Gender,
    string? PassportNo,
    DateTime? PassportExpirationDate,
    DateTime DateOfBirth,
    string? PlaceOfBirth,
    string? Address,
    string? PhoneNo,
    string? MdFileNo,
    RelationshipToFile? RelationshipToFile,
    string? ArsFolderNumber,
    // Residency Status
    PermitType? PermitType,
    DateTime? ResidencyIssueDate,
    ResidenceCategory? ResidenceCategory,
    string? PurposeOfResidence,
    DateTime? ResidencyExpiryDate,
    ResidencyStatus? ResidencyStatus,
    string? ResidencyDocumentNumber,
    // Residency Application
    PermitType? TypeOfPermitRequested,
    string? TypeOfApplication,
    DateTime? ApplicationSubmissionDate,
    ResidenceCategory? ApplicationResidenceCategory,
    string? ApplicationPurposeOfResidence,
    DateTime? ApplicationDecisionDate,
    ResidencyStatus? ApplicationStatus,
    // Return Decision
    DateTime? ReturnDecisionDate,
    string? ReturnDecisionText,
    int? VoluntaryReturnDeadlineDays,
    int? EntryBanDurationMonths
);

/// <summary>Response — Tables 152-155</summary>
public record ArsSearchResponse(
    // Main TCN Information (Table 152)
    string Arc,
    string FirstName,
    string LastName,
    Nationality Nationality,
    Gender Gender,
    string? PassportNo,
    DateTime? PassportExpirationDate,
    DateTime DateOfBirth,
    string? PlaceOfBirth,
    string? Address,
    string? PhoneNo,
    string? MdFileNo,
    RelationshipToFile? RelationshipToFile,
    string? ArsFolderNumber,
    // Residency Status (Table 153)
    ArsResidencyStatusDto? ResidencyStatus,
    // Residency Application (Table 154)
    ArsResidencyApplicationDto? ResidencyApplication,
    // Return Decision (Table 155)
    ArsReturnDecisionDto? ReturnDecision
);

public record ArsResidencyStatusDto(
    PermitType? PermitType,
    DateTime? IssueDate,
    ResidenceCategory? ResidenceCategory,
    string? PurposeOfResidence,
    DateTime? ExpiryDate,
    ResidencyStatus? Status,
    string? ResidencyDocumentNumber
);

public record ArsResidencyApplicationDto(
    PermitType? TypeOfPermitRequested,
    string? TypeOfApplication,
    DateTime? SubmissionDate,
    ResidenceCategory? ResidenceCategory,
    string? PurposeOfResidence,
    DateTime? DecisionDate,
    ResidencyStatus? Status
);

public record ArsReturnDecisionDto(
    DateTime? DecisionDate,
    string? DecisionText,
    int? VoluntaryReturnDeadlineDays,
    int? EntryBanDurationMonths
);

/// <summary>The dimension on which two or more ARS records were found to share an attribute.</summary>
public enum ArsDuplicateMatchType
{
    SameLastName,
    SameDateOfBirth,
    SameLastNameAndCountryOfOrigin,
    SameTravelDocumentIssuingCountry
}

/// <summary>A set of ARS records that share an attribute and may represent the same or a related person.</summary>
public record ArsDuplicateGroup(
    ArsDuplicateMatchType MatchType,
    string MatchKey,
    IReadOnlyList<ArsSearchResponse> Records
);

/// <summary>A set of ARS records that share the same ARS folder number (e.g. members of the same family/case file).</summary>
public record ArsFolderGroup(
    string ArsFolderNumber,
    IReadOnlyList<ArsSearchResponse> Records
);
