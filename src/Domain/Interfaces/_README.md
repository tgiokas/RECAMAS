# Repository interfaces

  - Repository interfaces live here (e.g. `ICaseRepository`, `ITCNProfileRepository`),
    implemented in `Infrastructure/Database`.
  - The Rule Engine's `IRuleEvaluator` also lives here — it's an in-process module,
    consumed by Case Management and Detention through an interface rather than a
    direct class reference, same as every other repository/evaluator contract.

External-service API client interfaces (`IStorageClient`, `INotificationClient`,
`IArsClient`, `ICassClient`, ...) live in `Application/Interfaces` instead, not
here — Domain shouldn't need to know HTTP exists. They're implemented in
`Infrastructure/ExternalClients`.

