# Reports module

Operational/audit reports, RECAMAS field-level RBAC config, document
templates, FAR export hook.

OPEN ITEM: AuditLog service is write-only (Kafka -> Elasticsearch), no query
API yet — this module's audit-trail view is blocked on that being defined.

Planned: ReportService, FieldPermissionService. Schema: reports.
