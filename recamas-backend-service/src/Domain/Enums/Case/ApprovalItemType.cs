namespace RECAMAS.Domain.Enums;

/// Τύπος approval item (§4.4.2.4)
public enum ApprovalItemType
{
    ReturnDecision,
    TravelDocIssuance,
    EntryBan,
    Escorts,
    OtherExpenses,
    MonetaryIncentive,
    Order,                          // FRC — DetentionOrder / DeportationOrder
    AlternativeMeasure              // FRC
}
