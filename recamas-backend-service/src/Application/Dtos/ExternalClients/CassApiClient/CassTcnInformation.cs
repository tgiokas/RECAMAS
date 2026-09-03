namespace RECAMAS.Application.Dtos.ExternalClients;

/// Table 158.
public sealed record CassTcnInformation(
    string? PassportNo,
    DateOnly? PassportExpirationDate,
    string? Address,
    string? PhoneNo,
    string? CassFileNo);
