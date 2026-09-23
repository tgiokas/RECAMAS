using RECAMAS.Application.Dtos.Interoperability.Shared;

namespace RECAMAS.Application.Interfaces.Interoperability;

public interface ITcnSearchOrchestrator
{
    Task<TcnSearchResult> SearchAsync(
        ExternalSearchRequest request,
        ExternalRequestContext context,
        CancellationToken cancellationToken = default);
}
