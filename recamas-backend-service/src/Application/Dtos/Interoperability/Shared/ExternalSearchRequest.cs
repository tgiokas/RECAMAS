namespace RECAMAS.Application.Dtos.Interoperability.Shared;

public sealed record ExternalSearchRequest(
    string? Arc,
    string? FirstName,
    string? LastName,
    DateOnly? DateOfBirth,
    string? NationalityCode,
    string? PassportNumber,
    DateOnly? DateOfBirthStart = null,
    DateOnly? DateOfBirthEnd = null,
    string? PassportIssuingCountry = null,
    string? TravelDocumentIssuingCountry = null,
    string? TravelDocumentType = null,
    string? Gender = null);
