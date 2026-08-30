using RECAMAS.Domain.Common;

namespace RECAMAS.Domain.Entities.Rules;

/// A named rule; the actual condition/action logic lives in its current
/// <see cref="RuleVersion"/> (architecture decision: "update" closes the old
/// version and inserts a new one — never mutates a version in place, so the
/// audit trail the original separate-microservice design was meant to
/// protect survives even though Rule Engine is now an in-process module).
/// Field-eligibility and IF/THEN syntax are specified in Specs 8.4.2/8.4.3 —
/// not modeled in this skeleton pass.
public class Rule : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }

    /// Null = applies to all case types.
    public Enums.CaseType? CaseType { get; set; }

    public List<RuleVersion> Versions { get; set; } = [];
}
