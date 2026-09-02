using RECAMAS.Application.Dtos.ExternalClients;

namespace RECAMAS.Application.Interfaces;

/// Contract for the reused Storage microservice — every RECAMAS document
/// (uploaded evidence, generated Return Decision/Detention Order templates,
/// JCC-signed PDFs) goes through this, never a local file system.

/// bucket/key rules): use a versioned key per document, e.g.
///   "{caseId}/{documentType}/v{n}.pdf"
/// so re-issued/re-signed documents don't collide with Storage's
/// duplicate-key rejection on upload./
public interface IStorageApiClient
{
    Task<StorageUploadResult?> UploadFileAsync(string bucket, string key,
         Stream fileStream, string fileName, string contentType,
         CancellationToken cancellationToken = default);
    Task<ResolvedFile?> DownloadAsync(string bucket, string key, CancellationToken cancellationToken = default);    
    Task<bool> DeleteFileAsync(string bucket, string key, CancellationToken cancellationToken = default);
}

/// Owns the underlying stream — dispose when done.
public sealed class ResolvedFile : IDisposable
{
    public required Stream Content { get; init; }
    public required string FileName { get; init; }
    public required string ContentType { get; init; }
    public long Size { get; init; }

    public void Dispose() => Content.Dispose();
}
