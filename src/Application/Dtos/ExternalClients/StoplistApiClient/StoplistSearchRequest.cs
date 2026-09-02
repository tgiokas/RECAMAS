namespace RECAMAS.Application.Dtos.ExternalClients;

/// Specs Table 167 (Stoplist Request Fields).
public sealed record StoplistSearchRequest(
    string? Arc,
    string? Name,
    string? Surname,
    string? Nationality,
    string? PassportNo,
    DateOnly? DateOfBirth);
