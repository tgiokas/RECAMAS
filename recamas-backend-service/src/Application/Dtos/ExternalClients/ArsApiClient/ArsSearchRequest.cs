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
