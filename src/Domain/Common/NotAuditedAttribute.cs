namespace RECAMAS.Domain.Common;

/// <summary>
/// Excludes one property's actual before/after value from the audit trail —
/// the change is still recorded as "this field changed", just without the
/// value. For properties where knowing a change happened is audit-relevant
/// but the value itself is too sensitive to carry through an outbox row and
/// Kafka topic in the clear (Study 12.5.7 requires sensitive data not be
/// logged). Property-level rather than the prototype's whole-entity
/// inclusion list, since most entities are a mix of sensitive and
/// unremarkable fields, not one or the other.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class NotAuditedAttribute : Attribute
{
}
