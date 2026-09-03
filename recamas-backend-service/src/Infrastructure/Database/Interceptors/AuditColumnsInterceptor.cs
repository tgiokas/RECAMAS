using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RECAMAS.Domain.Common;

namespace RECAMAS.Infrastructure.Database.Interceptors;

/// 
/// Fills the promise BaseEntity's own doc comment already made: "Audit columns
/// are populated by a SaveChanges interceptor in Infrastructure, not by
/// callers." Nothing implemented that promise until now — CreatedAt/CreatedBy
/// are set once on insert, UpdatedAt/UpdatedBy on every subsequent save.
/// 
public sealed class AuditColumnsInterceptor : SaveChangesInterceptor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuditColumnsInterceptor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        SetAuditColumns(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        SetAuditColumns(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void SetAuditColumns(DbContext? db)
    {
        if (db is null)
        {
            return;
        }

        var currentUser = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var now = DateTimeOffset.UtcNow;

        foreach (var entry in db.ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = now;
                entry.Entity.CreatedBy = currentUser;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = now;
                entry.Entity.UpdatedBy = currentUser;
            }
        }
    }
}
