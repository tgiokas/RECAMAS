using RECAMAS.Domain.Enums;

namespace RECAMAS.Application.Grids.TCNProfiles;

/// <summary>
/// Row projection for the TCN profiles grid, exposing a profile's identifying details,
/// demographics, status and risk flags. Supports server-side paging, sorting and filtering.
/// </summary>
public sealed class TCNProfileGridRowDto
{
    public long Id { get; set; }
    public string RecamasId { get; set; } = string.Empty;
    public string FirstNameEl { get; set; } = string.Empty;
    public string LastNameEl { get; set; } = string.Empty;
    public string FirstNameEn { get; set; } = string.Empty;
    public string LastNameEn { get; set; } = string.Empty;
    public GenderType Gender { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public TcnProfileStatus Status { get; set; }
    public DataSourceType PrimarySource { get; set; }
    public string Arc { get; set; } = string.Empty;
    public string MdFileNo { get; set; } = string.Empty;
    public string CassFileNo { get; set; } = string.Empty;
    public bool FlagSecurityIssues { get; set; }
    public bool FlagMinor { get; set; }
    public bool FlagNoArc { get; set; }
    public bool FlagNoTravelDocument { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
