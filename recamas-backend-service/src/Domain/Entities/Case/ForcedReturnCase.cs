using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.Case;

/// Forced Return case (§4.5)
public class ForcedReturnCase : ReturnCase
{
    public ForcedReturnProgram Program { get; set; }    // EURP | Cyprus

    // --- Apprehension & Preliminary Detention (§4.5.2.2) ---
    public string? ApprehensionOfficer { get; set; }    // Ονοματεπώνυμο — free text
    public string? ApprehensionLocation { get; set; }   // Τοποθεσία — free text (ανοιχτό)
    public DateTimeOffset? ApprehensionDateTime { get; set; }
    public string? ApprehensionJustification { get; set; } // Αιτιολόγηση — free text
    public string? PreliminaryDetentionLocation { get; set; } // Τοποθεσία — free text
    public EscapeRisk EscapeRisk { get; set; }          // Low | Medium | High
    public string? AiuOfficerNotes { get; set; }        // Σημειώσεις — free text
    public string? SuggestionMemoPath { get; set; }     // Storage path

    public PreReturnChecklist? PreReturnChecklist { get; set; }
    public ICollection<TravelDocumentIssuance> TravelDocumentIssuances { get; set; } = [];
    public ICollection<DetentionRecord> DetentionRecords { get; set; } = [];
    public ICollection<DetentionReassessment> Reassessments { get; set; } = [];
    public ICollection<DetentionCounsellingSession> CounsellingSessions { get; set; } = [];
    public ICollection<DetentionUpdateIssue> UpdatesIssues { get; set; } = [];
}
