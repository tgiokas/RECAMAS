# Rules module (Rule Engine)

In-process only — called directly by Case/Detention services, no HTTP client,
no separate deployable (see architecture decision log).

Rules are versioned: "update" in the UI closes the old RuleVersion's ValidTo
and inserts a new version rather than mutating in place, so past decisions
stay auditable. Conditions are a JSON AND/OR tree (see IRuleEvaluator).

Planned: RuleService, RuleEvaluator, ConditionTreeDto. Schema: rules.
