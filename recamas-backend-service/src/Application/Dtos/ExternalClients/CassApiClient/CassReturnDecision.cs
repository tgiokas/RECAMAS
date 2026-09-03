namespace RECAMAS.Application.Dtos.ExternalClients;

/// Table 162. See ArsReturnDecision remarks on the Number-typed deadline/ban fields.
public sealed record CassReturnDecision(
    DateOnly? DecisionDate,
    string? DecisionText,
    DateOnly? TcnReceiptDate,
    int? VoluntaryReturnDeadlineDays,
    int? EntryBanDurationMonths);
