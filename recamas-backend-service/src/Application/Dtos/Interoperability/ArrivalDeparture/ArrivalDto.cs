namespace RECAMAS.Application.Dtos.Interoperability.ArrivalDeparture;

public sealed record ArrivalDto(
    long? ArrId,
    long? IndIndId,
    DateOnly? ArrivalDate,
    string? PassportIssueCountry,
    string? PassportNo,
    string? VisaNo,
    long? DepDepId,
    int? ArrivalStatus);
