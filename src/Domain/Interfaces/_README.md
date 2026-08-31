# Repository interfaces

Convention (mirrors the `Authentication` service):
  - Repository interfaces live here (e.g. `ICaseRepository`, `ITCNProfileRepository`),
    implemented in `Infrastructure/Persistence`.
  - There is no `IDomainEventPublisher` here. The audit trail is handled by
    Cbs.Audit, which is audit-only, not a general pub/sub — a cross-module
    domain-event mechanism (for Notifications reacting to business events, say)
    will need its own design if one is ever needed; nothing currently provides it.
  - The Rule Engine's `IRuleEvaluator` also lives here — it's an in-process module,
    consumed by Case Management and Detention through an interface rather than a
    direct class reference, same as every other repository/evaluator contract.

External-service API client interfaces (`IStorageClient`, `INotificationClient`,
`IArsClient`, `ICassClient`, ...) live in `Application/Interfaces` instead, not
here — Domain shouldn't need to know HTTP exists. They're implemented in
`Infrastructure/ExternalClients`.

Rule: define the contract in Domain/Application first, implement in Infrastructure.
Never let a Controller or Application service `new()` up an Infrastructure class directly.
