namespace RECAMAS.Application.Dtos.Interoperability.Shared;

public sealed record ExternalServiceResult<T>(
    bool Success,
    string ServiceName,
    string Operation,
    T? Data,
    string CorrelationId,
    string? ErrorCode = null,
    string? ErrorMessage = null,
    int? StatusCode = null);
