namespace RECAMAS.Application.Interfaces;

/// <summary>
/// Contract for the reused Storage microservice — every RECAMAS document
/// (uploaded evidence, generated Return Decision/Detention Order templates,
/// JCC-signed PDFs) goes through this, never a local file system.
///
/// Key convention (open item, needs confirming against Storage's actual
/// bucket/key rules): use a versioned key per document, e.g.
///   "{caseId}/{documentType}/v{n}.pdf"
/// so re-issued/re-signed documents don't collide with Storage's
/// duplicate-key rejection on upload.
///
/// Lives here rather than Domain/Interfaces — see IAuthenticationClient remarks.
/// </summary>
public interface IStorageClient
{
    Task<string> UploadAsync(string bucketKey, Stream content, string contentType, CancellationToken ct = default);
    Task<Stream> DownloadAsync(string bucketKey, CancellationToken ct = default);
    Task DeleteAsync(string bucketKey, CancellationToken ct = default);
}
