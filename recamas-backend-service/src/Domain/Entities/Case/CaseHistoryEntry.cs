using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.Case;

/// Εγγραφή ιστορικού case (§4.3.8)
public class CaseHistoryEntry : BaseEntity
{
    public long CaseId { get; set; }

    public CaseHistoryEntryType EntryType { get; set; } // EntryCreation | DataUpdate | Assignment | Action | StageStatusChange | FlagChange
    public long? ActorUserId { get; set; }
    public long? ActorRoleId { get; set; }
    public bool IsSystemGenerated { get; set; }

    // DataUpdate
    public string? FieldName { get; set; }              // Όνομα πεδίου — string (reflection-based)
    public string? PreviousValue { get; set; }          // JSON serialized — string
    public string? UpdatedValue { get; set; }           // JSON serialized — string

    // Assignment
    public string? PreviousAssignment { get; set; }     // "UserName (RoleName)" — string
    public string? UpdatedAssignment { get; set; }

    // Action / StageStatus / Flag
    public string? ActionDescription { get; set; }      // Περιγραφή — free text

    // EntryCreation
    public string? EntryTypeName { get; set; }          // Τύπος εγγραφής — free text (πχ. "TravelDocument")

    public ReturnCase Case { get; set; } = null!;
}
