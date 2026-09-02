namespace RECAMAS.Application.Dtos.ExternalClients;

/// Table 154.
public sealed record ArsResidencyApplication(
    string? TypeOfPermitRequested,
    string? TypeOfApplication,
    DateOnly? SubmissionDate,
    string? ResidenceCategory,
    string? PurposeOfResidenceRnd,
    DateOnly? DecisionDate,
    string? Status);
