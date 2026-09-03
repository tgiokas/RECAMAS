namespace RECAMAS.Application.Dtos.ExternalClients;

/// Owns the underlying stream — dispose when done.
public sealed class StorageDownloadResult : IDisposable
{
    public required Stream Content { get; init; }
    public required string FileName { get; init; }
    public required string ContentType { get; init; }
    public long Size { get; init; }

    public void Dispose() => Content.Dispose();
}
