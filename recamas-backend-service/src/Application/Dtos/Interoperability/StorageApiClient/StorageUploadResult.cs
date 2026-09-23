namespace RECAMAS.Application.Dtos.Interoperability.StorageApiClient;

public record StorageUploadResult(string Bucket, string Key, string FileName, long FileSize);