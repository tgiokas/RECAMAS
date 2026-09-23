using System.Text.Json;
using System.Net;
using Microsoft.Extensions.Options;
using RECAMAS.Application.Configuration;
using RECAMAS.Application.Dtos.Interoperability.Shared;
using RECAMAS.Application.Interfaces.Interoperability;

namespace RECAMAS.Infrastructure.Interoperability.Adapters;

public sealed class CassAdapter(IOptions<CassInteroperabilitySettings> options) : IExternalServiceAdapter
{
    public string ServiceName => "CASS";

    public Task<JsonElement> ExecuteAsync(
        string operation,
        JsonElement requestData,
        ExternalRequestContext context,
        CancellationToken cancellationToken)
    {
        AdapterOperations.RequireSearch(operation);
        if (!options.Value.UseMock)
            throw new HttpRequestException("CASS is disabled until its WSDL is configured.", null, HttpStatusCode.ServiceUnavailable);

        var person = AdapterJson.Deserialize<ExternalPersonDto>(requestData);
        var result = person with { Source = "CASS", CassFileNo = "CASS-4455", IpStatus = "Pending" };
        return Task.FromResult(AdapterJson.Serialize<ExternalPersonDto?>(result));
    }
}
