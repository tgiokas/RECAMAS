namespace RECAMAS.Domain.Common;

/// 
/// Marker interface — entities implementing this have their Added/Modified/
/// Deleted changes automatically diffed and staged for the audit trail by
/// EntityChangeAuditInterceptor. Deliberately explicit opt-in per entity
/// (rather than "every BaseEntity is audited") because not everything needs
/// a change history, and blanket auditing would bury genuinely sensitive
/// changes (TCN case data) under noise from incidental lookup/join rows.
///
/// Fixes a known gap in the prototype this pattern is based on, which
/// hardcoded a single entity type (`e.Entity is Customer`) directly in the
/// interceptor — every module opts its own entities in here instead.
/// 
public interface IAuditable
{
}
