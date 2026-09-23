namespace RECAMAS.Application.Dtos.Interoperability.ArrivalDeparture;

public sealed record DepartureDto(
    long? DepId,
    long? IndIndId,
    DateOnly? DepartureDate,
    string? PassportIssueCountry,
    string? PassportNo,
    int? DepartureStatus);
