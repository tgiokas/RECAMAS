namespace RECAMAS.Application.Dtos.Interoperability.Shared;

public sealed record ExternalRequestContext(
    string CorrelationId,
    string TriggerType = "AdHoc",
    long? TriggeredByUserId = null,
    long? RelatedTcnProfileId = null,
    long? RelatedCaseId = null,
    string? Caller = null);
