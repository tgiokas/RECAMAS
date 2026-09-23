using Shared.Domain.Enums;

namespace CassApi.Application.DTOs;

public record CassSearchRequest(
    string? Arc, string? Name, string? Surname,
    Nationality? Nationality, string? PassportNo,
    DateTime? DateOfBirth, string? CassFileNo);

public record CassCreateRequest(
    string Arc, string FirstName, string LastName,
    Nationality Nationality, string? PassportNo,
    DateTime? PassportExpirationDate, DateTime DateOfBirth,
    string? Address, string? PhoneNo, string? CassFileNo,
    IpStatusType? IpStatusType, DateTime? IpStatusDateOfGranting,
    DateTime? IpStatusExpiryDate, DateTime? IpStatusDecisionDate,
    IpStatusDecision? IpStatusDecision,
    IpApplicationType? IpApplicationType, DateTime? IpApplicationSubmissionDate,
    DateTime? IpApplicationExpiryDate, DateTime? IpApplicationDecisionDate,
    IpStatusDecision? IpApplicationDecision,
    AppealType? AppealType, string? AppealNumber,
    DateTime? AppealDate, DateTime? AppealDecisionDate, AppealDecision? AppealDecision,
    DateTime? ReturnDecisionDate, string? ReturnDecisionText,
    DateTime? TcnReceiptDate, int? VoluntaryReturnDeadlineDays, int? EntryBanDurationMonths);

public record CassSearchResponse(
    string Arc, string? PassportNo, DateTime? PassportExpirationDate,
    string? Address, string? PhoneNo, string? CassFileNo,
    CassIpStatusDto? IpStatus,
    CassIpApplicationDto? IpApplication,
    CassAppealDto? Appeal,
    CassReturnDecisionDto? ReturnDecision);

public record CassIpStatusDto(IpStatusType? TypeOfStatus,
    DateTime? DateOfGranting, DateTime? ExpiryDate,
    DateTime? DecisionDate, IpStatusDecision? StatusDecision);

public record CassIpApplicationDto(IpApplicationType? TypeOfApplication,
    DateTime? SubmissionDate, DateTime? ExpiryDate,
    DateTime? DecisionDate, IpStatusDecision? StatusDecision);

public record CassAppealDto(AppealType? TypeOfAppeal, string? AppealNumber,
    DateTime? AppealDate, DateTime? DecisionDate, AppealDecision? StatusDecision);

public record CassReturnDecisionDto(DateTime? DecisionDate, string? DecisionText,
    DateTime? TcnReceiptDate, int? VoluntaryReturnDeadlineDays, int? EntryBanDurationMonths);
