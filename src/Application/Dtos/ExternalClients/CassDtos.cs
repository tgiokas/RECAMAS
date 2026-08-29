namespace RECAMAS.Application.Dtos.ExternalClients;

/// <summary>Study Table 157 (CASS Request Fields).</summary>
public sealed record CassSearchRequest(
    string? Arc,
    string? Name,
    string? Surname,
    string? Nationality,
    string? PassportNo,
    DateOnly? DateOfBirth,
    string? CassFileNo);

/// <summary>Study Tables 158-162 (CASS Response Fields).</summary>
public sealed record CassSearchResult(
    CassTcnInformation TcnInformation,
    CassIpStatus? IpStatus,
    IReadOnlyList<CassIpApplication> IpApplications,
    IReadOnlyList<CassAppeal> Appeals,
    CassReturnDecision? ReturnDecision);

/// <summary>Table 158.</summary>
public sealed record CassTcnInformation(
    string? PassportNo,
    DateOnly? PassportExpirationDate,
    string? Address,
    string? PhoneNo,
    string? CassFileNo);

/// <summary>Table 159.</summary>
public sealed record CassIpStatus(
    string? TypeOfStatus,
    DateOnly? DateOfGranting,
    DateOnly? ExpiryDate,
    DateOnly? DecisionDate,
    string? StatusDecision);

/// <summary>Table 160.</summary>
public sealed record CassIpApplication(
    string? TypeOfApplication,
    DateOnly? SubmissionDate,
    DateOnly? ExpiryDate,
    DateOnly? DecisionDate,
    string? StatusDecision);

/// <summary>Table 161.</summary>
public sealed record CassAppeal(
    string? TypeOfAppeal,
    string? AppealNumber,
    DateOnly? AppealDate,
    DateOnly? DecisionDate,
    string? AppealStatusDecision);

/// <summary>Table 162. See ArsReturnDecision remarks on the Number-typed deadline/ban fields.</summary>
public sealed record CassReturnDecision(
    DateOnly? DecisionDate,
    string? DecisionText,
    DateOnly? TcnReceiptDate,
    int? VoluntaryReturnDeadlineDays,
    int? EntryBanDurationMonths);
