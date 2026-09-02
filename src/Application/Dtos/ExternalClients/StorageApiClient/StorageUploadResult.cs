namespace RECAMAS.Application.Dtos;

public record StorageUploadResult(string Bucket, string Key, string FileName, long FileSize);