using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.Case;

/// Κεντρική οντότητα case — base class για AVR, ForcedReturn, ByOwn (§4)
public class ReturnCase : BaseEntity
{
    public string CaseId { get; set; } = null!;                  // Human-readable system-generated ID (πχ. AVR-2026-001)
    public CaseType CaseType { get; set; }              // AVR | ForcedReturn | ByOwn
    public CaseStage Stage { get; set; }                // Counselling | ApplicationProcessing | Detention | κλπ
    public CaseStatus Status { get; set; }              // Initiated | OnHold | PendingApproval | κλπ

    public DateTimeOffset InitiationDateTime { get; set; } // System-generated κατά δημιουργία

    public InitiationOffice? InitiationOffice { get; set; } // MD | συγκεκριμένο A&IU office
    public string? ImplementationOffice { get; set; }   // A&IU office υλοποίησης — free text (codelist-driven από admin)

    public string? ReturnCountryCode { get; set; }      // ISO 3166-1 alpha-2 — string (standard country code)
    public ReturnReason? ReturnReason { get; set; }     // ILMigrant | AsylumSeeker | AsylumRejection | κλπ
    public InternationalFramework? InternationalFramework { get; set; } // EUReadmission | Bilateral | κλπ

    // --- Flags (§4.3.4) ---
    public bool FlagNoArc { get; set; }
    public bool FlagNoTravelDocument { get; set; }
    public bool FlagMinor { get; set; }
    public bool FlagUnaccompaniedMinor { get; set; }
    public bool FlagCriminalRecord { get; set; }
    public bool FlagRestrictiveActivities { get; set; }
    public bool FlagHealthIssues { get; set; }
    public bool FlagNeedsAttention { get; set; }        // Interface update σε TCN profile (auto On Hold)
    public bool FlagException { get; set; }             // Χώρα εκτός προγράμματος
    public bool FlagProgramSwitch { get; set; }         // AVR Cyprus ↔ EURP switch

    // --- On Hold / Cancel ---
    public bool IsOnHold { get; set; }
    public OnHoldReason? OnHoldReason { get; set; }     // ManuallyPlaced | InterfaceUpdate
    public bool IsCancelled { get; set; }
    public string? CancellationReason { get; set; }     // Ελεύθερο κείμενο — απαιτείται (§4.3.2)
    public DateTimeOffset? CancelledAt { get; set; }

    // --- Relationships ---
    public ICollection<CaseAssignment> Assignments { get; set; } = [];
    public ICollection<CaseRequest> Requests { get; set; } = [];
    public ICollection<CaseHistoryEntry> History { get; set; } = [];
    public ICollection<CaseDocument> Documents { get; set; } = [];
    public ICollection<ApprovalItem> ApprovalItems { get; set; } = [];
    public ICollection<AdjustmentNote> AdjustmentNotes { get; set; } = [];
    public ICollection<CaseTcn> CaseTcns { get; set; } = [];
    public long? ReturnImplementationId { get; set; }
    public RECAMAS.Domain.Entities.ReturnImplementation.ReturnImplementation? ReturnImplementation { get; set; }
}
