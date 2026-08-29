using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using RECAMAS.Application.Interfaces;
using RECAMAS.Infrastructure.ApiClients;

namespace RECAMAS.Infrastructure.ExternalClients;

/// <summary>
/// Typed HttpClient for the reused Storage service. Stub only —
/// real upload/download endpoint contracts to be confirmed against
/// Storage's actual API before this is used for real case documents.
/// </summary>
public class StorageClient : ApiClientBase, IStorageClient
{
    public StorageClient(HttpClient httpClient, ILogger<StorageClient> logger)
        : base(httpClient, logger)
    {
    }

    public async Task<string> UploadAsync(string bucketKey, Stream content, string contentType, CancellationToken ct = default)
    {
        using var streamContent = new StreamContent(content);
        streamContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

        // TODO: confirm actual Storage upload endpoint + response shape.
        var request = new HttpRequestMessage(HttpMethod.Post, $"/api/storage/{Uri.EscapeDataString(bucketKey)}")
        {
            Content = streamContent,
        };

        var response = await SendRequestAsync(request, ct);
        response.EnsureSuccessStatusCode();
        return bucketKey;
    }

    public async Task<Stream> DownloadAsync(string bucketKey, CancellationToken ct = default)
    {
        // TODO: confirm actual Storage download endpoint.
        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/storage/{Uri.EscapeDataString(bucketKey)}");
        var response = await SendRequestAsync(request, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStreamAsync(ct);
    }

    public async Task DeleteAsync(string bucketKey, CancellationToken ct = default)
    {
        // TODO: confirm actual Storage delete endpoint.
        var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/storage/{Uri.EscapeDataString(bucketKey)}");
        var response = await SendRequestAsync(request, ct);
        response.EnsureSuccessStatusCode();
    }
}
