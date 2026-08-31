# Repository interfaces

Convention (mirrors the `Authentication` service):
  - Repository interfaces live here (e.g. `ICaseRepository`, `ITCNProfileRepository`),
    implemented in `Infrastructure/Persistence`.
  - There is no `IDomainEventPublisher` here anymore — the outbox/Kafka pipeline
    it fronted was removed when the project switched to Cbs.Audit (a package)
    for the audit trail (see architecture decision log). Cbs.Audit is audit-only,
    not a general pub/sub, so a cross-module domain-event mechanism (for
    Notifications reacting to business events, say) will need a new design if
    one is ever needed again — nothing currently provides it.
  - The Rule Engine's `IRuleEvaluator` also lives here — even though Rule Engine is
    an in-process module (not a separate microservice), it's still consumed by
    Case Management and Detention through an interface, not a direct class reference,
    so it stays easy to extract into a real microservice later if that's ever needed.

External-service API client interfaces (`IStorageClient`, `INotificationClient`,
`IArsClient`, `ICassClient`, ...) live in `Application/Interfaces` instead, not
here — Domain shouldn't need to know HTTP exists. They're implemented in
`Infrastructure/ExternalClients`.

Rule: define the contract in Domain/Application first, implement in Infrastructure.
Never let a Controller or Application service `new()` up an Infrastructure class directly.
