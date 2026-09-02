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
