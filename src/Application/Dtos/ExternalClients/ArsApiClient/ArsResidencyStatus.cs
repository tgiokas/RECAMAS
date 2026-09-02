namespace RECAMAS.Application.Dtos.ExternalClients;

/// Table 153.
public sealed record ArsResidencyStatus(
    string? PermitType,
    DateOnly? IssueDate,
    string? ResidenceCategory,
    string? PurposeOfResidenceRnd,
    DateOnly? ExpiryDate,
    string? Status,
    string? ResidencyDocumentNumber);
