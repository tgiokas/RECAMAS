namespace RECAMAS.Application.Dtos.ExternalClients;

/// Table 152. Photograph intentionally omitted here — see TCNProfile remarks on Photograph/Fingerprints.
public sealed record ArsTcnInformation(
    string? Arc,
    string? FirstName,
    string? LastName,
    string? Nationality,
    string? Gender,
    string? PassportNo,
    DateOnly? PassportExpirationDate,
    DateOnly? DateOfBirth,
    string? PlaceOfBirth,
    string? Address,
    string? PhoneNo,
    string? MdFileNo,
    string? RelationshipToMdFile);
