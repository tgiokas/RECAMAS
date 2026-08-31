# Entities

Convention: one file per entity, entity name matches file name, all inherit
`RECAMAS.Domain.Common.BaseEntity`. Group related entities in a subfolder per
module only if the module ends up with more than ~5 entities
(e.g. `Entities/Case/`, `Entities/Detention/`) — keep it flat until then.
