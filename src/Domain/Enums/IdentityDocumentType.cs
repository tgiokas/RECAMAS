namespace RECAMAS.Domain.Enums;

/// <summary>
/// Per Implementation Study Table 4 (Profile fields - Identity Information):
/// "Document Type | Enum | ... | Passport, Country Issued ID, Other".
/// </summary>
public enum IdentityDocumentType
{
    Passport = 1,
    CountryIssuedId = 2,
    Other = 3,
}
