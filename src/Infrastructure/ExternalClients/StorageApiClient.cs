using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;

using RECAMAS.Application.Dtos.ExternalClients;
using RECAMAS.Application.Interfaces;
using RECAMAS.Infrastructure.ApiClients;

namespace RECAMAS.Infrastructure.ExternalClients;

/// HttpClient for the reused Storage service. 
public class StorageApiClient : ApiClientBase, IStorageApiClient
{
    private const string UploadEndpoint = $"/Documents/upload";
    private const string DownloadEndpoint = "/Documents/download";
    private const string DeleteEndpoint = $"/Documents/delete";

    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public StorageApiClient(HttpClient httpClient, ILogger<StorageApiClient> logger)
        : base(httpClient, logger)
    {
    }

    public async Task<StorageUploadResult?> UploadFileAsync(
        string bucket, string key,
        Stream fileStream, string fileName, string contentType,
        CancellationToken cancellationToken = default)
    {
        using var content = new MultipartFormDataContent();
        var streamContent = new StreamContent(fileStream);
        streamContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
        content.Add(streamContent, "file", fileName);
        content.Add(new StringContent(bucket), "bucket");
        content.Add(new StringContent(key), "key");

        var request = new HttpRequestMessage(HttpMethod.Post, UploadEndpoint)
        {
            Content = content
        };

        var response = await SendRequestAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
            return null;

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        var result = JsonSerializer.Deserialize<StorageApiResponse>(json, JsonOptions);

        if (result is null || !result.Success || result.Data is null)
        {
            _logger.LogError("DMS.Storage returned unsuccessful response for {Bucket}/{Key}", bucket, key);
            return null;
        }

        return new StorageUploadResult(
            result.Data.Bucket,
            result.Data.Key,
            fileName,
            result.Data.Size);
    }

    public async Task<ResolvedFile?> DownloadAsync(string bucket, string key, CancellationToken cancellationToken = default)
    {
        var url = $"{DownloadEndpoint}?bucket={Uri.EscapeDataString(bucket)}&key={Uri.EscapeDataString(key)}";

        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        using var response = await SendRequestAsync(request, cancellationToken);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            return null;
        }

        var contentType = response.Content.Headers.ContentType?.MediaType ?? "application/octet-stream";

        static string? Blank(string? s) => string.IsNullOrWhiteSpace(s) ? null : s;

        var fileName = Blank(response.Content.Headers.ContentDisposition?.FileNameStar)
                       ?? Blank(response.Content.Headers.ContentDisposition?.FileName?.Trim('"'))
                       ?? Blank(key.TrimEnd('/').Split('/').LastOrDefault())
                       ?? "attachment";

        var bytes = await response.Content.ReadAsByteArrayAsync(cancellationToken);

        return new ResolvedFile
        {
            Content = new MemoryStream(bytes, 0, bytes.Length, writable: false, publiclyVisible: true),
            FileName = fileName,
            ContentType = contentType,
            Size = bytes.LongLength
        };
    }

    public async Task<bool> DeleteFileAsync(string bucket, string key, CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, DeleteEndpoint)
            {
                Content = JsonContent.Create(new { bucket, key })
            };

            var response = await SendRequestAsync(request, cancellationToken);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to delete orphaned file {Bucket}/{Key} from DMS.Storage", bucket, key);
            return false;
        }
    }
}
