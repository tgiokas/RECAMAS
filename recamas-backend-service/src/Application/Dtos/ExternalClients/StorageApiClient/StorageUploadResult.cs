namespace RECAMAS.Application.Dtos.ExternalClients;

public record StorageUploadResult(string Bucket, string Key, string FileName, long FileSize);