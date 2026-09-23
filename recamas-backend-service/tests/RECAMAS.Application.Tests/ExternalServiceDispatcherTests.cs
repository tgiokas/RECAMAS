using System.Text.Json;
using RECAMAS.Application.Dtos.Interoperability.Shared;
using RECAMAS.Application.Errors;
using RECAMAS.Application.Interfaces.Interoperability;
using RECAMAS.Application.Modules.Interoperability;

namespace RECAMAS.Application.Tests;

public sealed class ExternalServiceDispatcherTests
{
    [Fact]
    public async Task ExecuteAsync_RoutesToAdapterAndDeserializesTypedResponse()
    {
        var adapter = new StubAdapter("ARS", new ExternalPersonDto(
            "ARS", "A-1", null, null, "Ada", "Lovelace", new DateOnly(1815, 12, 10), "GBR", null));
        var dispatcher = new ExternalServiceDispatcher([adapter]);

        var result = await dispatcher.ExecuteAsync<ExternalPersonDto>(
            "ars", "Search", new { arc = "A-1" }, new ExternalRequestContext("corr-1"));

        Assert.True(result.Success);
        Assert.Equal("A-1", result.Data?.Arc);
        Assert.Equal("corr-1", result.CorrelationId);
        Assert.Equal(1, adapter.CallCount);
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsStructuredFailureForUnknownService()
    {
        var dispatcher = new ExternalServiceDispatcher([]);

        var result = await dispatcher.ExecuteAsync<object>(
            "UNKNOWN", "Search", new { }, new ExternalRequestContext("corr-2"));

        Assert.False(result.Success);
        Assert.Equal(ErrorCodes.Interoperability.ServiceNotFound, result.ErrorCode);
        Assert.Equal(502, result.StatusCode);
    }

    private sealed class StubAdapter(string serviceName, object result) : IExternalServiceAdapter
    {
        public string ServiceName => serviceName;
        public int CallCount { get; private set; }

        public Task<JsonElement> ExecuteAsync(string operation, JsonElement requestData, ExternalRequestContext context, CancellationToken cancellationToken)
        {
            CallCount++;
            return Task.FromResult(JsonSerializer.SerializeToElement(result, new JsonSerializerOptions(JsonSerializerDefaults.Web)));
        }
    }
}
