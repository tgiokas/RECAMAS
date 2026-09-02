namespace RECAMAS.Application.Dtos.ExternalClients;

/// Table 155. VoluntaryReturnDeadline/EntryBanDuration are typed "Number" in the
/// Specs rather than "Date" (unlike the equivalent Table 3/10 profile-level
/// fields, which are dates) — kept as int? here to match the interface's own
/// wire type; likely a day-count the Application layer turns into a real date.
public sealed record ArsReturnDecision(
    DateOnly? DecisionDate,
    string? DecisionText,
    int? VoluntaryReturnDeadlineDays,
    int? EntryBanDurationMonths);
