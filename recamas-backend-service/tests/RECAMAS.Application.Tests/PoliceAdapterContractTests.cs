using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RECAMAS.Application.Configuration;
using RECAMAS.Application.Dtos.Interoperability.Shared;
using RECAMAS.Application.Dtos.Interoperability.StopList;
using RECAMAS.Infrastructure.Interoperability.Adapters;

namespace RECAMAS.Application.Tests;

public sealed class PoliceAdapterContractTests
{
    [Fact]
    public async Task StoplistAdapter_UsesDocumentedRouteHeadersAndRequestShape()
    {
        var handler = new CapturingHandler("{\"hitFound\":true}");
        var adapter = new StoplistAdapter(
            new HttpClient(handler),
            Options.Create(new PoliceInteroperabilitySettings
            {
                BaseUrl = "https://police.example/",
                ApiKey = "secret",
                ClientId = "recamas",
            }));
        var search = new ExternalSearchRequest(
            null, "Ada", "Lovelace", new DateOnly(1815, 12, 10), "GBR", "P-1",
            TravelDocumentIssuingCountry: "GBR", TravelDocumentType: "P", Gender: "F");

        var json = System.Text.Json.JsonSerializer.SerializeToElement(search, new System.Text.Json.JsonSerializerOptions(System.Text.Json.JsonSerializerDefaults.Web));
        var response = await adapter.ExecuteAsync("Search", json, new ExternalRequestContext("corr-police"), CancellationToken.None);
        var result = response.Deserialize<StoplistSearchResult>(new System.Text.Json.JsonSerializerOptions(System.Text.Json.JsonSerializerDefaults.Web));

        Assert.True(result?.HitFound);
        Assert.Equal("https://police.example/police/police-checks/v1/stoplist/search", handler.RequestUri?.ToString());
        Assert.Equal("secret", handler.Headers["api-key"]);
        Assert.Equal("recamas", handler.Headers["X-Client-ID"]);
        Assert.Equal("corr-police", handler.Headers["X-Correlation-ID"]);
        Assert.Contains("\"exactDate\":\"1815-12-10\"", handler.Body);
        Assert.Contains("\"travelDocumentNumber\":\"P-1\"", handler.Body);
    }

    private sealed class CapturingHandler(string responseJson) : HttpMessageHandler
    {
        public Uri? RequestUri { get; private set; }
        public Dictionary<string, string> Headers { get; } = new(StringComparer.OrdinalIgnoreCase);
        public string Body { get; private set; } = string.Empty;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            RequestUri = request.RequestUri;
            foreach (var header in request.Headers)
                Headers[header.Key] = header.Value.Single();
            Body = request.Content is null ? string.Empty : await request.Content.ReadAsStringAsync(cancellationToken);
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseJson, Encoding.UTF8, "application/json"),
            };
        }
    }
}
