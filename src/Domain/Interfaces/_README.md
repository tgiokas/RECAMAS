# Repository interfaces

Convention (mirrors the `Authentication` service):
  - Repository interfaces live here (e.g. `ICaseRepository`, `ITCNProfileRepository`),
    implemented in `Infrastructure/Persistence`.
  - `IDomainEventPublisher` lives here too — domain entities/services raise domain
    events, so the port belongs with them even though Kafka implements it.
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
