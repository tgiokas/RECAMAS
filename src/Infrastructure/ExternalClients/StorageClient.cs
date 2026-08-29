using RECAMAS.Domain.Interfaces;

namespace RECAMAS.Infrastructure.ExternalClients;

/// <summary>
/// Typed HttpClient for the reused Storage service. Stub only —
/// real upload/download endpoint contracts to be confirmed against
/// Storage's actual API before this is used for real case documents.
/// </summary>
public class StorageClient : IStorageClient
{
    private readonly HttpClient _httpClient;

    public StorageClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> UploadAsync(string bucketKey, Stream content, string contentType, CancellationToken ct = default)
    {
        using var streamContent = new StreamContent(content);
        streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);

        // TODO: confirm actual Storage upload endpoint + response shape.
        var response = await _httpClient.PostAsync($"/api/storage/{Uri.EscapeDataString(bucketKey)}", streamContent, ct);
        response.EnsureSuccessStatusCode();
        return bucketKey;
    }

    public async Task<Stream> DownloadAsync(string bucketKey, CancellationToken ct = default)
    {
        // TODO: confirm actual Storage download endpoint.
        return await _httpClient.GetStreamAsync($"/api/storage/{Uri.EscapeDataString(bucketKey)}", ct);
    }

    public async Task DeleteAsync(string bucketKey, CancellationToken ct = default)
    {
        // TODO: confirm actual Storage delete endpoint.
        var response = await _httpClient.DeleteAsync($"/api/storage/{Uri.EscapeDataString(bucketKey)}", ct);
        response.EnsureSuccessStatusCode();
    }
}
