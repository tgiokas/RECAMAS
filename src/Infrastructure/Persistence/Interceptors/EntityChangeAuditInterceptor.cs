using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RECAMAS.Domain.Common;
using RECAMAS.Domain.Entities.Outbox;
using RECAMAS.Infrastructure.Helpers.Redaction;

namespace RECAMAS.Infrastructure.Persistence.Interceptors;

/// <summary>
/// Adapted from the CustomerAudit.Api prototype's AuditSaveChangesInterceptor,
/// with its 3 flagged gaps fixed:
///  - Entity selection was hardcoded (`e.Entity is Customer`) — here it's the
///    <see cref="IAuditable"/> marker interface, so every module opts its own
///    entities in.
///  - No redaction — here, <see cref="NotAuditedAttribute"/> excludes a
///    property's actual value from the diff entirely (change is still
///    recorded, value isn't), and JsonRedactor runs as a generic-keyword
///    backstop over whatever JSON does get built.
///  - No retry cap — that lives in OutboxProcessor, not here (see its remarks).
/// Tamper-evidence (the audit trail's own access being auditable, an ES
/// index policy denying update/delete) is infrastructure/ops the reused
/// AuditLog service owns, not something this interceptor can provide.
///
/// Writes an OutboxMessage to the SAME DbContext mid-SaveChanges, so it
/// commits atomically with the business change it describes — the entire
/// point of doing this as an interceptor rather than a separate step.
///
/// ONE CONFIRMED-BY-TESTING NUANCE — present in the prototype too, not
/// introduced here: for a newly Added entity with a database-generated
/// identity key (every entity in this app), the key's CurrentValue at
/// SavingChanges time is EF Core's temporary placeholder, not the real
/// value Postgres assigns on INSERT — confirmed by running this against a
/// real database, where a CREATE event's EntityId showed up as
/// "-9223372036854774807" instead of the actual new row's id. Fixed below
/// by tracking those cases (PropertyEntry.IsTemporary) and patching just
/// the EntityId/Key columns in SavedChangesAsync once the real value is
/// known — a small follow-up write, not a second attempt at the audit
/// record itself, so the core guarantee (the record exists, commits
/// atomically with the business row, has the right diff content) isn't
/// weakened. The embedded payload JSON's own "after.Id" field is left
/// showing the placeholder for CREATE events — a cosmetic gap, not
/// re-serializing the payload to fix a display-only field isn't worth the
/// added complexity.
/// </summary>
public sealed class EntityChangeAuditInterceptor : SaveChangesInterceptor
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly List<(EntityEntry Entry, OutboxMessage Message)> _pendingKeyFixups = [];

    public EntityChangeAuditInterceptor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        AddAuditOutboxMessages(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        AddAuditOutboxMessages(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        FixUpGeneratedKeys(eventData.Context);
        return base.SavedChanges(eventData, result);
    }

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        await FixUpGeneratedKeysAsync(eventData.Context, cancellationToken);
        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    private void AddAuditOutboxMessages(DbContext? db)
    {
        _pendingKeyFixups.Clear();

        if (db is null)
        {
            return;
        }

        var auditableEntries = db.ChangeTracker.Entries()
            .Where(e => e.Entity is IAuditable && e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
            .ToList();

        if (auditableEntries.Count == 0)
        {
            return;
        }

        var http = _httpContextAccessor.HttpContext;
        var userId = http?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userName = http?.User.Identity?.Name;
        var correlationId = http?.TraceIdentifier;

        foreach (var entry in auditableEntries)
        {
            var message = BuildMessage(entry, userId, userName, correlationId, out var keyIsTemporary);
            db.Set<OutboxMessage>().Add(message);

            if (keyIsTemporary)
            {
                _pendingKeyFixups.Add((entry, message));
            }
        }
    }

    private void FixUpGeneratedKeys(DbContext? db) => FixUpGeneratedKeysAsync(db, default).GetAwaiter().GetResult();

    private async Task FixUpGeneratedKeysAsync(DbContext? db, CancellationToken ct)
    {
        if (db is null || _pendingKeyFixups.Count == 0)
        {
            return;
        }

        foreach (var (entry, message) in _pendingKeyFixups)
        {
            var pkProperty = entry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey());
            var realId = pkProperty?.CurrentValue?.ToString();

            if (string.IsNullOrEmpty(realId) || message.Id == 0)
            {
                continue;
            }

            await db.Set<OutboxMessage>()
                .Where(m => m.Id == message.Id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(m => m.EntityId, realId)
                    .SetProperty(m => m.Key, $"{message.EntityType}:{realId}"), ct);
        }

        _pendingKeyFixups.Clear();
    }

    private static OutboxMessage BuildMessage(
        EntityEntry entry, string? userId, string? userName, string? correlationId, out bool keyIsTemporary)
    {
        var action = entry.State switch
        {
            EntityState.Added => "CREATE",
            EntityState.Modified => "UPDATE",
            EntityState.Deleted => "DELETE",
            _ => entry.State.ToString().ToUpperInvariant(),
        };

        var before = new Dictionary<string, object?>();
        var after = new Dictionary<string, object?>();
        var changed = new List<string>();

        foreach (var property in entry.Properties)
        {
            var name = property.Metadata.Name;
            var isNotAudited = property.Metadata.PropertyInfo?.GetCustomAttributes(typeof(NotAuditedAttribute), inherit: true).Length > 0;

            if (entry.State == EntityState.Added)
            {
                changed.Add(name);
                if (!isNotAudited)
                {
                    after[name] = property.CurrentValue;
                }
            }
            else if (entry.State == EntityState.Deleted)
            {
                changed.Add(name);
                if (!isNotAudited)
                {
                    before[name] = property.OriginalValue;
                }
            }
            else if (property.IsModified)
            {
                changed.Add(name);
                if (!isNotAudited)
                {
                    before[name] = property.OriginalValue;
                    after[name] = property.CurrentValue;
                }
            }
        }

        var pkProperty = entry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey());
        keyIsTemporary = entry.State == EntityState.Added && (pkProperty?.IsTemporary ?? false);
        var id = pkProperty?.CurrentValue?.ToString() ?? pkProperty?.OriginalValue?.ToString();

        var entityType = entry.Metadata.ClrType.Name;

        var payload = JsonSerializer.Serialize(new
        {
            before = before.Count == 0 ? null : before,
            after = after.Count == 0 ? null : after,
            changedProperties = changed,
        });

        return new OutboxMessage
        {
            EventType = "audit.entity.changed",
            Category = "Entity",
            Payload = JsonRedactor.TryRedact(payload),
            Key = $"{entityType}:{id}",
            UserId = userId,
            UserName = userName,
            EntityType = entityType,
            EntityId = id,
            CorrelationId = correlationId,
        };
    }
}
