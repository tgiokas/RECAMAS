using System.Text.Json;
using RECAMAS.Application.Dtos.Interoperability.Shared;

namespace RECAMAS.Application.Interfaces.Interoperability;

public interface IExternalServiceAdapter
{
    string ServiceName { get; }

    Task<JsonElement> ExecuteAsync(
        string operation,
        JsonElement requestData,
        ExternalRequestContext context,
        CancellationToken cancellationToken);
}
