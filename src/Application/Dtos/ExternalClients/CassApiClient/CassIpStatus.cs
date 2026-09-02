namespace RECAMAS.Application.Dtos.ExternalClients;

/// Table 159.
public sealed record CassIpStatus(
    string? TypeOfStatus,
    DateOnly? DateOfGranting,
    DateOnly? ExpiryDate,
    DateOnly? DecisionDate,
    string? StatusDecision);
