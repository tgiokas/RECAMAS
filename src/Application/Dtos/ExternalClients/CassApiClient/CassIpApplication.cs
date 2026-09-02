namespace RECAMAS.Application.Dtos.ExternalClients;

/// Table 160.
public sealed record CassIpApplication(
    string? TypeOfApplication,
    DateOnly? SubmissionDate,
    DateOnly? ExpiryDate,
    DateOnly? DecisionDate,
    string? StatusDecision);
