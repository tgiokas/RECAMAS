namespace RECAMAS.Application.Dtos.Interoperability.Shared;

public sealed record ExternalPersonDto(
    string Source,
    string? Arc,
    string? ArsFolderNumber,
    string? CassFileNo,
    string? FirstName,
    string? LastName,
    DateOnly? DateOfBirth,
    string? NationalityCode,
    string? PassportNumber,
    string? Gender = null,
    string? PassportIssuingCountry = null,
    string? TravelDocumentType = null,
    string? IpStatus = null);
