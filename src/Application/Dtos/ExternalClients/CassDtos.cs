namespace RECAMAS.Application.Dtos.ExternalClients;

/// Specs Table 157 (CASS Request Fields).
public sealed record CassSearchRequest(
    string? Arc,
    string? Name,
    string? Surname,
    string? Nationality,
    string? PassportNo,
    DateOnly? DateOfBirth,
    string? CassFileNo);

/// Specs Tables 158-162 (CASS Response Fields).
public sealed record CassSearchResult(
    CassTcnInformation TcnInformation,
    CassIpStatus? IpStatus,
    IReadOnlyList<CassIpApplication> IpApplications,
    IReadOnlyList<CassAppeal> Appeals,
    CassReturnDecision? ReturnDecision);

/// Table 158.
public sealed record CassTcnInformation(
    string? PassportNo,
    DateOnly? PassportExpirationDate,
    string? Address,
    string? PhoneNo,
    string? CassFileNo);

/// Table 159.
public sealed record CassIpStatus(
    string? TypeOfStatus,
    DateOnly? DateOfGranting,
    DateOnly? ExpiryDate,
    DateOnly? DecisionDate,
    string? StatusDecision);

/// Table 160.
public sealed record CassIpApplication(
    string? TypeOfApplication,
    DateOnly? SubmissionDate,
    DateOnly? ExpiryDate,
    DateOnly? DecisionDate,
    string? StatusDecision);

/// Table 161.
public sealed record CassAppeal(
    string? TypeOfAppeal,
    string? AppealNumber,
    DateOnly? AppealDate,
    DateOnly? DecisionDate,
    string? AppealStatusDecision);

/// Table 162. See ArsReturnDecision remarks on the Number-typed deadline/ban fields.
public sealed record CassReturnDecision(
    DateOnly? DecisionDate,
    string? DecisionText,
    DateOnly? TcnReceiptDate,
    int? VoluntaryReturnDeadlineDays,
    int? EntryBanDurationMonths);
