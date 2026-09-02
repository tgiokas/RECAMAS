namespace RECAMAS.Application.Dtos.ExternalClients;

/// Specs Table 151 (ARS Request Fields).
public sealed record ArsSearchRequest(
    string? Arc,
    string? Name,
    string? Surname,
    string? Nationality,
    string? PassportNo,
    DateOnly? DateOfBirth,
    string? MdFileNumber);

/// Specs Tables 152-155 (ARS Response Fields) — one search call returns all four blocks.
public sealed record ArsSearchResult(
    ArsTcnInformation TcnInformation,
    ArsResidencyStatus? ResidencyStatus,
    IReadOnlyList<ArsResidencyApplication> ResidencyApplications,
    ArsReturnDecision? ReturnDecision);

/// Table 152. Photograph intentionally omitted here — see TCNProfile remarks on Photograph/Fingerprints.
public sealed record ArsTcnInformation(
    string? Arc,
    string? FirstName,
    string? LastName,
    string? Nationality,
    string? Gender,
    string? PassportNo,
    DateOnly? PassportExpirationDate,
    DateOnly? DateOfBirth,
    string? PlaceOfBirth,
    string? Address,
    string? PhoneNo,
    string? MdFileNo,
    string? RelationshipToMdFile);

/// Table 153.
public sealed record ArsResidencyStatus(
    string? PermitType,
    DateOnly? IssueDate,
    string? ResidenceCategory,
    string? PurposeOfResidenceRnd,
    DateOnly? ExpiryDate,
    string? Status,
    string? ResidencyDocumentNumber);

/// Table 154.
public sealed record ArsResidencyApplication(
    string? TypeOfPermitRequested,
    string? TypeOfApplication,
    DateOnly? SubmissionDate,
    string? ResidenceCategory,
    string? PurposeOfResidenceRnd,
    DateOnly? DecisionDate,
    string? Status);

/// Table 155. VoluntaryReturnDeadline/EntryBanDuration are typed "Number" in the
/// Specs rather than "Date" (unlike the equivalent Table 3/10 profile-level
/// fields, which are dates) — kept as int? here to match the interface's own
/// wire type; likely a day-count the Application layer turns into a real date.
public sealed record ArsReturnDecision(
    DateOnly? DecisionDate,
    string? DecisionText,
    int? VoluntaryReturnDeadlineDays,
    int? EntryBanDurationMonths);
