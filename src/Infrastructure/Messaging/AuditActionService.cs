using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using RECAMAS.Application.Interfaces;
using RECAMAS.Domain.Entities.Outbox;
using RECAMAS.Domain.Interfaces;

namespace RECAMAS.Infrastructure.Messaging;

public class AuditActionService : IAuditActionService
{
    private readonly IOutboxRepository _outboxRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuditActionService(IOutboxRepository outboxRepository, IHttpContextAccessor httpContextAccessor)
    {
        _outboxRepository = outboxRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task RecordActionAsync(
        string action, string? category = null, string? entityType = null, string? entityId = null,
        object? metadata = null, CancellationToken ct = default)
    {
        var http = _httpContextAccessor.HttpContext;

        var message = new OutboxMessage
        {
            EventType = "audit.action." + action,
            Category = category ?? "Business",
            Payload = JsonSerializer.Serialize(new { action, metadata }),
            UserId = http?.User.FindFirstValue(ClaimTypes.NameIdentifier),
            UserName = http?.User.Identity?.Name,
            EntityType = entityType,
            EntityId = entityId,
            CorrelationId = http?.TraceIdentifier,
        };

        await _outboxRepository.AddAsync(message, ct);
    }
}
