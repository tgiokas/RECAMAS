namespace RECAMAS.Domain.Interfaces;

/// <summary>
/// Contract for the reused Notifications microservice. Unlike Authentication/Storage,
/// this is NOT called over HTTP — Notifications is a pure Kafka consumer. RECAMAS
/// publishes to the same topic pattern it already consumes
/// (notifications.email.{auth|backend|citizen}, or a new notifications.email.recamas
/// topic — open item, see architecture decision log), via IAuditEventPublisher below.
///
/// This interface intentionally has no methods yet — kept here as a placeholder so
/// the module list / DI registration reflects "Notifications integration exists"
/// even though, structurally, RECAMAS never calls it directly.
/// </summary>
public interface INotificationClient
{
    // Deliberately empty: RECAMAS -> Notifications is one-way via Kafka
    // (see IDomainEventPublisher), not a request/response HTTP client.
}
