# Repository & external-service interfaces

Empty by design for this skeleton commit.

Convention (mirrors the `Authentication` service):
  - Repository interfaces live here (e.g. `ICaseRepository`, `ITCNProfileRepository`),
    implemented in `Infrastructure/Persistence`.
  - External-service client interfaces also live here (e.g. `IAuthenticationClient`,
    `IStorageClient`, `INotificationClient`, `IAuditEventPublisher`), implemented in
    `Infrastructure/ExternalClients` and `Infrastructure/Messaging`.
  - The Rule Engine's `IRuleEvaluator` also lives here — even though Rule Engine is
    an in-process module (not a separate microservice), it's still consumed by
    Case Management and Detention through an interface, not a direct class reference,
    so it stays easy to extract into a real microservice later if that's ever needed.

Rule: define the contract in Domain/Application first, implement in Infrastructure.
Never let a Controller or Application service `new()` up an Infrastructure class directly.
