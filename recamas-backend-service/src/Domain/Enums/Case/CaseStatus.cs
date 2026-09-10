namespace RECAMAS.Domain.Enums;

/// Status case (§4.4.1 / §4.5.1 / §4.6.1)
public enum CaseStatus
{
    Initiated,
    PendingSecurityCheck,
    SecurityCheckCompleted,
    AdjustmentsNeeded,
    OnHold,
    Cancelled,
    PendingApproval,
    Rejected,
    PendingOrders,                  // FRC
    PendingCheckin,                 // FRC
    InDetention,                    // FRC
    ReadyForDeparture,              // FRC
    PendingTravelDocument,          // AVR
    PendingSignOff,
    PendingTicket,                  // AVR
    DepartureInitiated,             // AVR
    Departed,
    InProgress,
    Completed,
    Closed
}
