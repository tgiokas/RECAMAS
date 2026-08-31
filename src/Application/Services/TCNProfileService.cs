using System.Text.Json;
using Microsoft.Extensions.Logging;
using RECAMAS.Application.Common;
using RECAMAS.Application.Dtos.TCNProfile;
using RECAMAS.Application.Errors;
using RECAMAS.Application.Events;
using RECAMAS.Application.Interfaces;
using RECAMAS.Domain.Entities.Outbox;
using RECAMAS.Domain.Interfaces;
using TCNProfileEntity = RECAMAS.Domain.Entities.TCNProfile.TCNProfile;

namespace RECAMAS.Application.Services;

/// Reference implementation for the full Controller -> Service -> Repository ->
/// Postgres -> Outbox -> Kafka flow (see architecture decision log). CreateAsync
/// writes two outbox rows directly via IOutboxRepository.AddWithoutSaveAsync,
/// deliberately bypassing IAuditActionService/IDomainEventPublisher: both of
/// those commit on their own (their own doc comments flag this as a scope
/// limit), which would mean "profile created" could get recorded without the
/// profile actually existing, or vice versa, if the process died between two
/// separate commits. Writing directly here keeps the profile insert and both
/// outbox rows inside the single transaction SaveChangesAsync opens — the
/// actual guarantee the outbox pattern exists for. Those two abstractions
/// remain the right choice for events that AREN'T tied to a specific entity
/// write in the same operation (e.g. a standalone access-log entry with
/// nothing else to commit alongside).
public class TCNProfileService : ITCNProfileService
{
    private readonly ITCNProfileRepository _tcnProfileRepository;
    private readonly IOutboxRepository _outboxRepository;
    private readonly IApplicationDbContext _dbContext;
    private readonly IErrorCatalog _errors;
    private readonly ILogger<TCNProfileService> _logger;

    public TCNProfileService(
        ITCNProfileRepository tcnProfileRepository,
        IOutboxRepository outboxRepository,
        IApplicationDbContext dbContext,
        IErrorCatalog errors,
        ILogger<TCNProfileService> logger)
    {
        _tcnProfileRepository = tcnProfileRepository;
        _outboxRepository = outboxRepository;
        _dbContext = dbContext;
        _errors = errors;
        _logger = logger;
    }

    public async Task<Result<TCNProfileDto>> CreateAsync(CreateTCNProfileRequest request, CancellationToken ct = default)
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

        var profile = new TCNProfileEntity
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

        // Explicit business-action audit entry — same transaction as the profile insert.
        var actionMessage = new OutboxMessage
        {
            EventType = "audit.action.tcn.profile.registered",
            Category = "Business",
            Payload = JsonSerializer.Serialize(new { profile.Arc, profile.FirstNameEn, profile.LastNameEn }),
            EntityType = "TCNProfile", // nameof(TCNProfileEntity) would give the local alias name, not the entity's actual type name
            EntityId = profile.PublicId.ToString(),
        };
        await _outboxRepository.AddWithoutSaveAsync(actionMessage, ct);

        // Explicit integration event for other consumers (Notifications/AuditLog) — same transaction.
        var domainEvent = new TCNProfileCreatedEvent(profile.PublicId!.Value, profile.Arc, profile.FirstNameEn, profile.LastNameEn);
        var eventMessage = new OutboxMessage
        {
            EventType = nameof(TCNProfileCreatedEvent),
            Key = profile.PublicId.ToString(),
            Payload = JsonSerializer.Serialize(domainEvent),
            EntityType = "TCNProfile", // nameof(TCNProfileEntity) would give the local alias name, not the entity's actual type name
            EntityId = profile.PublicId.ToString(),
        };
        await _outboxRepository.AddWithoutSaveAsync(eventMessage, ct);

        // Commits the profile insert and both outbox rows together. EntityChangeAuditInterceptor
        // also fires here automatically, adding a third outbox row (audit.entity.changed) to the
        // same SaveChanges call — nothing in this method has to ask for that one.
        await _dbContext.SaveChangesAsync(ct);

        _logger.LogInformation("TCN Profile {PublicId} created", profile.PublicId);

        return Result<TCNProfileDto>.Ok(MapToDto(profile), "TCN Profile created successfully.");
    }

    private static TCNProfileDto MapToDto(TCNProfileEntity profile) => new()
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
