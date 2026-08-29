namespace RECAMAS.Application.Dtos.ExternalClients;

/// Study Table 167 (Stoplist Request Fields).
public sealed record StoplistSearchRequest(
    string? Arc,
    string? Name,
    string? Surname,
    string? Nationality,
    string? PassportNo,
    DateOnly? DateOfBirth);

/// Study Table 168 (Stoplist Response Fields). See IStoplistClient remarks
/// on the live-API-vs-batch-file contradiction (9.5 vs 12.3.7).
public sealed record StoplistCheckResult(
    bool StoplistHit,
    string? UniqueEntryBanNumber,
    DateOnly? StoplistEntryDate);
