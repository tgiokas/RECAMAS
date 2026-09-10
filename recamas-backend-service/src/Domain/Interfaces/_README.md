# Repository interfaces

  - Repository interfaces live here only when an aggregate needs focused query
    behavior, such as `ITcnProfileRepository`. EF Core's `DbContext` remains the
    unit-of-work boundary; there is no custom generic repository or Unit of Work.
  - Implementations live in `Infrastructure/Repositories`.
  - The Rule Engine's `IRuleEvaluator` also lives here — it's an in-process module,
    consumed by Case Management and Detention through an interface rather than a
    direct class reference, same as every other repository/evaluator contract.

External-service API client interfaces (`IStorageClient`, `INotificationClient`,
`IArsClient`, `ICassClient`, ...) live in `Application/Interfaces` instead, not
here — Domain shouldn't need to know HTTP exists. They're implemented in
`Infrastructure/ExternalClients`.

