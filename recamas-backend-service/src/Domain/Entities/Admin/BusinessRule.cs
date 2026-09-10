using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.Admin;

/// Κανόνας BRE (§8.4)
public class BusinessRule : BaseEntity
{
    public string RuleId { get; set; } = null!;                  // Human-readable system ID — string
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public CaseType WorkflowType { get; set; }
    public string? Programme { get; set; }              // Programme code ή null (= AllProgrammes) — string: codelist value

    // Transition (§8.4.1)
    public CaseStage FromStage { get; set; }
    public CaseStatus FromStatus { get; set; }
    public CaseStage ToStage { get; set; }
    public CaseStatus ToStatus { get; set; }

    // IF condition (§8.4.2)
    public string ConditionExpression { get; set; } = null!;     // JSON DSL — string (structured expression tree)

    // THEN action (§8.4.3)
    public BREThenAction ThenAction { get; set; }
    public string? TargetFieldOrDocumentType { get; set; } // Field name ή doc type — string (dynamic)
    public string UserFacingMessage { get; set; } = null!;       // Μήνυμα — free text

    // Priority & Lifecycle (§8.4.4 / §8.4.5)
    public int Priority { get; set; }
    public DateTimeOffset ValidFrom { get; set; }
    public DateTimeOffset? ValidTo { get; set; }
    public BREStatus RuleStatus { get; set; }           // Draft | Scheduled | Effective | Expired

    // Version history
    public long? ClonedFromRuleId { get; set; }
    public bool IsClone { get; set; }

    public BusinessRule? ClonedFromRule { get; set; }
}
