using RECAMAS.Domain.Common;

namespace RECAMAS.Domain.Entities.Rules;

/// One version of a Rule's condition/action logic. ConditionsJson is the
/// "structured JSON tree, nested AND/OR groups of field-operator-value"
/// stored as Postgres jsonb (see RuleVersionConfiguration).
/// ThenActionsJson mirrors Specs 8.4.3.1's supported THEN actions, same
/// treatment. Only one version per Rule should have IsActive=true at a time,
/// enforcing that is Application-layer work (RuleService), not a DB constraint here.
public class RuleVersion : BaseEntity
{
    public long RuleId { get; set; }

    public int VersionNumber { get; set; }

    public string ConditionsJson { get; set; } = "{}";
    public string ThenActionsJson { get; set; } = "[]";

    public int Priority { get; set; }
    public bool IsActive { get; set; }

    public DateTimeOffset EffectiveFrom { get; set; }
    public DateTimeOffset? EffectiveTo { get; set; }
}
