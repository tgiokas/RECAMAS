using Microsoft.Extensions.Logging;

using RECAMAS.Application.Common;
using RECAMAS.Application.Dtos.TCNProfile;
using RECAMAS.Application.Errors;
using RECAMAS.Application.Interfaces;
using RECAMAS.Domain.Interfaces;
using RECAMAS.Domain.Entities.TCNProfile;

namespace RECAMAS.Application.Modules;

/// Reference implementation for the full Controller -> Service -> Repository ->
/// Postgres -> Cbs.Audit flow. Creating the profile is all this method does —
/// TCNProfile's [Audited] attribute (see its own remarks) makes
/// AddEntityAuditing<ApplicationDbContext>'s interceptor capture the
/// TCNPROFILE.CREATED audit event automatically the moment SaveChangesAsync
/// runs, with no manual outbox write needed here.
///
/// OPEN ITEM: Cbs.Audit is an audit-trail mechanism, not a general pub/sub —
/// it has no facility for notifying other consumers (e.g. Notifications
/// reacting to "a profile was created"). If/when a module needs that, a
/// separate mechanism will need to be introduced; nothing here provides it.
public class TCNProfileService : ITCNProfileService
{
    private readonly ITCNProfileRepository _tcnProfileRepository;
    private readonly IApplicationDbContext _dbContext;
    private readonly IErrorCatalog _errors;
    private readonly ILogger<TCNProfileService> _logger;

    public TCNProfileService(
        ITCNProfileRepository tcnProfileRepository,
        IApplicationDbContext dbContext,
        IErrorCatalog errors,
        ILogger<TCNProfileService> logger)
    {
        _tcnProfileRepository = tcnProfileRepository;
        _dbContext = dbContext;
        _errors = errors;
        _logger = logger;
    }

    public async Task<Result<TCNProfileDto>> CreateAsync(TCNProfileCreateRequest request, CancellationToken ct = default)
    {
        var duplicates = await _tcnProfileRepository.SearchForDuplicatesAsync(
            request.Arc, passportNumber: null, request.FirstNameEn, request.LastNameEn, ct);

        if (duplicates.Count > 0)
        {
            _logger.LogInformation(
                "Rejected TCN Profile creation for {FirstName} {LastName}: {Count} possible duplicate(s) found",
                request.FirstNameEn, request.LastNameEn, duplicates.Count);
            return _errors.Fail<TCNProfileDto>(ErrorCodes.TCNProfile.DuplicateProfileDetected);
        }

        var profile = new TCNProfile
        {
            PublicId = Guid.NewGuid(),
            Arc = request.Arc,
            FirstNameEl = request.FirstNameEl,
            FirstNameEn = request.FirstNameEn,
            LastNameEl = request.LastNameEl,
            LastNameEn = request.LastNameEn,
            Gender = request.Gender,
            DateOfBirth = request.DateOfBirth,
            PlaceOfBirth = request.PlaceOfBirth,
        };

        await _tcnProfileRepository.AddWithoutSaveAsync(profile, ct);

        // Cbs.Audit's SaveChanges interceptor captures TCNPROFILE.CREATED here automatically.
        await _dbContext.SaveChangesAsync(ct);

        _logger.LogInformation("TCN Profile {PublicId} created", profile.PublicId);

        return Result<TCNProfileDto>.Ok(MapToDto(profile), "TCN Profile created successfully.");
    }

    private static TCNProfileDto MapToDto(TCNProfile profile) => new()
    {
        PublicId = profile.PublicId!.Value,
        DisplayCode = profile.DisplayCode,
        Arc = profile.Arc,
        FirstNameEn = profile.FirstNameEn,
        LastNameEn = profile.LastNameEn,
        Gender = profile.Gender?.ToString(),
        DateOfBirth = profile.DateOfBirth,
        Age = profile.Age,
    };
}
