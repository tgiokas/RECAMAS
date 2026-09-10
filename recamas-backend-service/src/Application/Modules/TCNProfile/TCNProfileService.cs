using Microsoft.Extensions.Logging;

using RECAMAS.Application.Dtos.Common;
using RECAMAS.Application.Dtos.TCNProfile;
using RECAMAS.Application.Errors;
using RECAMAS.Application.Interfaces;
using RECAMAS.Domain.Interfaces;
using RECAMAS.Domain.Entities.TCNProfile;

namespace RECAMAS.Application.Modules;

/// Reference implementation for the Controller -> Service -> Repository ->
/// Postgres flow. Creating the profile is all this method does.
///
/// OPEN ITEM: Cbs.Audit is an audit-trail mechanism, not a general pub/sub —
/// it has no facility for notifying other consumers (e.g. Notifications
/// reacting to "a profile was created"). If/when a module needs that, a
/// separate mechanism will need to be introduced; nothing here provides it.
public class TCNProfileService : ITCNProfileService
{
    private readonly ITcnProfileRepository _tcnProfileRepository;
    private readonly IApplicationDbContext _dbContext;
    private readonly IErrorCatalog _errors;
    private readonly ILogger<TCNProfileService> _logger;

    public TCNProfileService(
        ITcnProfileRepository tcnProfileRepository,
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

        var profile = new TcnProfile
        {
            PublicId = Guid.NewGuid(),
            RecamasId = $"TCN-{DateTime.UtcNow:yyyy}-{Guid.NewGuid():N}"[..22],
            Arc = request.Arc,
            FirstNameEl = request.FirstNameEl,
            FirstNameEn = request.FirstNameEn,
            LastNameEl = request.LastNameEl,
            LastNameEn = request.LastNameEn,
            Gender = request.Gender!.Value,
            DateOfBirth = request.DateOfBirth,
            PlaceOfBirth = request.PlaceOfBirth,
        };

        await _tcnProfileRepository.AddAsync(profile, ct);

        // Cbs.Audit's SaveChanges interceptor captures TCNPROFILE.CREATED here automatically.
        await _dbContext.SaveChangesAsync(ct);

        _logger.LogInformation("TCN Profile {PublicId} created", profile.PublicId);

        return Result<TCNProfileDto>.Ok(MapToDto(profile), "TCN Profile created successfully.");
    }

    private static TCNProfileDto MapToDto(TcnProfile profile) => new()
    {
        PublicId = profile.PublicId,
        DisplayCode = profile.RecamasId,
        Arc = profile.Arc,
        FirstNameEn = profile.FirstNameEn,
        LastNameEn = profile.LastNameEn,
        Gender = profile.Gender.ToString(),
        DateOfBirth = profile.DateOfBirth,
        Age = CalculateAge(profile.DateOfBirth),
    };

    private static int? CalculateAge(DateOnly? dateOfBirth)
    {
        if (dateOfBirth is null) return null;
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var age = today.Year - dateOfBirth.Value.Year;
        return dateOfBirth.Value > today.AddYears(-age) ? age - 1 : age;
    }
}
