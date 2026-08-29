# Entities

Empty by design for this skeleton commit.

Populated module-by-module in the next phase (e.g. `TCNProfile.cs`, `Case.cs`,
`AvrCaseDetails.cs`, `DetentionOrder.cs`, `Rule.cs`, `RuleVersion.cs` ...).

Convention: one file per entity, entity name matches file name, all inherit
`RECAMAS.Domain.Common.BaseEntity`. Group related entities in a subfolder per
module only if the module ends up with more than ~5 entities
(e.g. `Entities/Case/`, `Entities/Detention/`) — keep it flat until then.
