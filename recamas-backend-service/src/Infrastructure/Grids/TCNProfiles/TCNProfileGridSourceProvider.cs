using RECAMAS.Application.Grids.Services;
using RECAMAS.Application.Grids.TCNProfiles;
using RECAMAS.Application.Interfaces;
using RECAMAS.Domain.Entities.TCNProfile;

namespace RECAMAS.Infrastructure.Grids.TCNProfiles;

/// <summary>
/// Grid source provider that supplies TCN profile rows for the TCN profiles data grid.
/// Soft-deleted profiles are already excluded by the global query filter on <c>IsDeleted</c>.
/// </summary>
public sealed class TCNProfileGridSourceProvider : GridSourceProvider<TCNProfileGridRowDto>
{
    public override string? RequiredPermission => "tcnprofiles";

    public TCNProfileGridSourceProvider(TCNProfileGridConfiguration configuration)
        : base(configuration, BuildQuery)
    {
    }

    private static IQueryable<TCNProfileGridRowDto> BuildQuery(IApplicationDbContext dbContext)
    {
        return from profile in dbContext.Set<TcnProfile>()
               select new TCNProfileGridRowDto
               {
                   Id = profile.Id,
                   RecamasId = profile.RecamasId,
                   FirstNameEl = profile.FirstNameEl ?? string.Empty,
                   LastNameEl = profile.LastNameEl ?? string.Empty,
                   FirstNameEn = profile.FirstNameEn ?? string.Empty,
                   LastNameEn = profile.LastNameEn ?? string.Empty,
                   Gender = profile.Gender,
                   DateOfBirth = profile.DateOfBirth,
                   Status = profile.Status,
                   PrimarySource = profile.PrimarySource,
                   Arc = profile.Arc ?? string.Empty,
                   MdFileNo = profile.MdFileNo ?? string.Empty,
                   CassFileNo = profile.CassFileNo ?? string.Empty,
                   FlagSecurityIssues = profile.FlagSecurityIssues,
                   FlagMinor = profile.FlagMinor,
                   FlagNoArc = profile.FlagNoArc,
                   FlagNoTravelDocument = profile.FlagNoTravelDocument,
                   CreatedAt = profile.CreatedAt
               };
    }
}
