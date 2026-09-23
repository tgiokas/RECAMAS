using Shared.Domain.Common;
using Shared.Domain.Enums;

namespace CassApi.Domain.Entities;

/// <summary>
/// CASS record — Cyprus Asylum Service system.
/// Mirrors §9.3 Implementation Study (Tables 157-162).
/// </summary>
public class CassRecord : BaseEntity
{
    // === IDENTIFICATION ===
    public string Arc { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public Nationality Nationality { get; set; }
    public string? PassportNo { get; set; }
    public DateTime? PassportExpirationDate { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Address { get; set; }
    public string? PhoneNo { get; set; }
    public string? CassFileNo { get; set; }

    // === IP STATUS (Table 159) ===
    public IpStatusType? IpStatusType { get; set; }
    public DateTime? IpStatusDateOfGranting { get; set; }
    public DateTime? IpStatusExpiryDate { get; set; }
    public DateTime? IpStatusDecisionDate { get; set; }
    public IpStatusDecision? IpStatusDecision { get; set; }

    // === IP APPLICATION (Table 160) ===
    public IpApplicationType? IpApplicationType { get; set; }
    public DateTime? IpApplicationSubmissionDate { get; set; }
    public DateTime? IpApplicationExpiryDate { get; set; }
    public DateTime? IpApplicationDecisionDate { get; set; }
    public IpStatusDecision? IpApplicationDecision { get; set; }

    // === APPEAL (Table 161) ===
    public AppealType? AppealType { get; set; }
    public string? AppealNumber { get; set; }
    public DateTime? AppealDate { get; set; }
    public DateTime? AppealDecisionDate { get; set; }
    public AppealDecision? AppealDecision { get; set; }

    // === RETURN DECISION (Table 162) ===
    public DateTime? ReturnDecisionDate { get; set; }
    public string? ReturnDecisionText { get; set; }
    public DateTime? TcnReceiptDate { get; set; }
    public int? VoluntaryReturnDeadlineDays { get; set; }
    public int? EntryBanDurationMonths { get; set; }
}
