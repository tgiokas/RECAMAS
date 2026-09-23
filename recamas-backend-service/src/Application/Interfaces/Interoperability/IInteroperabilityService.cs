using RECAMAS.Application.Dtos.Interoperability.Shared;

namespace RECAMAS.Application.Interfaces.Interoperability;

public interface IInteroperabilityService
{
    Task<ExternalServiceResult<TResponse>> ExecuteAsync<TResponse>(
        string serviceName,
        string operation,
        object request,
        ExternalRequestContext context,
        CancellationToken cancellationToken = default);
}
