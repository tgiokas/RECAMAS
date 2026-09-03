namespace RECAMAS.Domain.Enums;

/// Per Implementation Specs Table 4 (Profile fields - Identity Information):
/// "Document Type | Enum | ... | Passport, Country Issued ID, Other".
public enum IdentityDocumentType
{
    Passport = 1,
    CountryIssuedId = 2,
    Other = 3,
}
