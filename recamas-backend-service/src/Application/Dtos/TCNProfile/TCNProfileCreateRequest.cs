using RECAMAS.Domain.Enums;

namespace RECAMAS.Application.Dtos.TCNProfile;

/// Overview-tab core fields only (Study Table 3) — enough to prove the
/// Controller -> Service -> Repository -> Postgres -> flow
/// end to end. Identity documents, nationalities, and the other 10 child
/// collections are a separate "add detail" flow
public class TCNProfileCreateRequest
{
    public string? Arc { get; init; }
    public string? FirstNameEl { get; init; }
    public required string FirstNameEn { get; init; }
    public string? LastNameEl { get; init; }
    public required string LastNameEn { get; init; }
    public Gender? Gender { get; init; }
    public DateOnly? DateOfBirth { get; init; }
    public string? PlaceOfBirth { get; init; }
}
