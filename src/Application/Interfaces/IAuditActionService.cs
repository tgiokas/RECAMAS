namespace RECAMAS.Application.Interfaces;

/// <summary>
/// For audit-worthy actions that aren't entity CRUD — logins, approvals,
/// case-stage transitions triggered by business logic rather than a direct
/// field edit — and so aren't captured by EntityChangeAuditInterceptor.
/// Adapted from the CustomerAudit.Api prototype's AuditActionService.
/// </summary>
public interface IAuditActionService
{
    Task RecordActionAsync(
        string action,
        string? category = null,
        string? entityType = null,
        string? entityId = null,
        object? metadata = null,
        CancellationToken ct = default);
}
