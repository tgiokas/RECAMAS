# Entities

Convention: one file per entity, entity name matches file name, and all persisted
entities inherit `RECAMAS.Domain.Common.BaseEntity`. Entities are grouped by the
database module/schema: `TCNProfile`, `Case`, `Detention`, `ReturnImplementation`,
and `Admin`.
