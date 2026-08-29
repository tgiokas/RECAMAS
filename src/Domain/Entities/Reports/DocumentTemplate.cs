using RECAMAS.Domain.Common;

namespace RECAMAS.Domain.Entities.Reports;

/// One document template, keyed by CaseType — Study Table 20 lists 10 concrete
/// templates today (7 AVR: Return Decision, Counselling Report, Declaration of
/// Voluntary Departure, Certificate of Participation, Consent to Travel with
/// One Parent, Consent to Monetary Incentive Deduction, Affidavit for Appeals;
/// 3 Forced Return: Suggestion Memo, Detention Order, Deportation Order).
/// Rows aren't seeded by this skeleton — that's a data migration, not a schema
/// concern. TemplateFileDocumentId points at the actual template file in the
/// Storage service, not at bytes stored here.
public class DocumentTemplate : BaseEntity
{
    public Enums.CaseType CaseType { get; set; }

    public required string Title { get; set; }
    public string? Description { get; set; }

    public Guid? TemplateFileDocumentId { get; set; }
}
