using Microsoft.Extensions.Configuration;

namespace RECAMAS.Application.Configuration;

public class StorageClientSettings
{
    public required string BaseUrl { get; init; }

    public static StorageClientSettings BindFromConfiguration(IConfiguration configuration)
    {
        return new StorageClientSettings
        {
            BaseUrl = configuration["STORAGE_BASE_URL"]
                ?? throw new InvalidOperationException("STORAGE_BASE_URL is not configured."),
        };
    }
}
