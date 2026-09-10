namespace RECAMAS.Domain.Enums;

/// Τύπος notification event (§11.3)
public enum NotificationEventType
{
    CaseAssigned,                   // §11.3.2
    CaseSubmittedToStage,           // §11.3.2
    TravelDocDeliveredToOffice,     // §11.3.2
    PreReturnChecklistComplete,     // §11.3.2
    NeedsAttentionOnHold,           // §11.3.2 / §11.3.3 / §11.3.4
    NewApprovalItemDecided,         // §11.3.2 / §11.3.4
    RequestAddressed,               // §11.3.2 / §11.3.3 / §11.3.4 / §11.3.5
    RequestReplyReceived,           // §11.3.2 / §11.3.3 / §11.3.4 / §11.3.5
    UnassignedCaseAgeing,           // §11.3.2
    SecurityCheckPending,           // §11.3.3 / §11.3.5
    SecurityCheckCompleted,         // §11.3.3
    TravelDocIssuanceApproved,      // §11.3.3
    AdjustmentsNeeded,              // §11.3.3
    ArcCreatedFlagCleared,          // §11.3.3
    CaseSubmittedFromCounselling,   // §11.3.3
    ReassessmentDueOrOverdue,       // §11.3.4
    TcnStatusUpdateDuringDetention, // §11.3.4
    DetentionMilestoneFlagReached,  // §11.3.4
    OrderPreparationPending,        // §11.3.5
    ItemsPendingApproval,           // §11.3.6
    ApprovalAgeing,                 // §11.3.6
    AddedAsApprover,                // §11.3.6
    EurpTicketUploaded,             // §11.3.1
    InterfaceExecutionFailure,      // §11.3.9
    ConfigurationPublished,         // §11.3.9
    UserLifecycleEvent              // §11.3.9
}
