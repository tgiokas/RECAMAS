namespace RECAMAS.Application.Dtos.TCNProfile;

public class TCNProfileDto
{
    public required Guid PublicId { get; init; }
    public string? DisplayCode { get; init; }
    public string? Arc { get; init; }
    public string? FirstNameEn { get; init; }
    public string? LastNameEn { get; init; }
    public string? Gender { get; init; }
    public DateOnly? DateOfBirth { get; init; }
    public int? Age { get; init; }
}
