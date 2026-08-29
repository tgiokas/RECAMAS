namespace RECAMAS.Application.Dtos.ExternalClients;

/// <summary>Study Table 167 (Stoplist Request Fields).</summary>
public sealed record StoplistSearchRequest(
    string? Arc,
    string? Name,
    string? Surname,
    string? Nationality,
    string? PassportNo,
    DateOnly? DateOfBirth);

/// <summary>
/// Study Table 168 (Stoplist Response Fields). See IStoplistClient remarks
/// on the live-API-vs-batch-file contradiction (9.5 vs 12.3.7).
/// </summary>
public sealed record StoplistCheckResult(
    bool StoplistHit,
    string? UniqueEntryBanNumber,
    DateOnly? StoplistEntryDate);
