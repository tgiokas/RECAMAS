namespace RECAMAS.Application.Dtos.Interoperability.Shared;

public sealed record DuplicateCandidateDto(
    Guid PublicId,
    string RecamasId,
    string? Arc,
    string? FirstName,
    string? LastName,
    DateOnly? DateOfBirth);
