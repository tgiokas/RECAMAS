namespace RECAMAS.Application.Dtos.ExternalClients;

/// Table 161.
public sealed record CassAppeal(
    string? TypeOfAppeal,
    string? AppealNumber,
    DateOnly? AppealDate,
    DateOnly? DecisionDate,
    string? AppealStatusDecision);
