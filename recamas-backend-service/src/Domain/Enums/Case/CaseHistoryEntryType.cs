namespace RECAMAS.Domain.Enums;

/// Τύπος εγγραφής ιστορικού (§4.3.8)
public enum CaseHistoryEntryType
{
    EntryCreation,
    DataUpdate,
    Assignment,
    Action,
    StageStatusChange,
    FlagChange
}
