using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.ReturnImplementation;

/// Υλοποίηση επιστροφής — Single AVR/ByOwn/Forced ή Group Forced (§6)
public class ReturnImplementation : BaseEntity
{
    public string ImplementationId { get; set; } = null!;        // System-generated — string
    public ImplementationType ImplementationType { get; set; }
    public DateTimeOffset InitiationDateTime { get; set; }
    public DateOnly? PlannedExecutionDate { get; set; }
    public string ReturnCountryCode { get; set; } = null!;       // ISO 3166-1 alpha-2 — string (standard country code)
    public string? TransitCountryCodes { get; set; }    // JSON array of ISO codes — string[] serialized
    public ImplementationStatus Status { get; set; }

    // Group Forced only
    public OperationOwner? OperationOwner { get; set; } // FRONTEX | CyprusAuthorities | Other
    public string? FlightNumber { get; set; }           // Free text (airline format)

    public string? CancellationReason { get; set; }     // Free text

    public ICollection<ReturnCase> LinkedCases { get; set; } = [];
    public ICollection<ImplementationTcn> ImplementationTcns { get; set; } = [];
    public ICollection<ImplementationEscortTeamMember> EscortTeam { get; set; } = [];
    public ICollection<ImplementationOtherExpense> OtherExpenses { get; set; } = [];
    public ICollection<ImplementationDocument> Documents { get; set; } = [];
}
