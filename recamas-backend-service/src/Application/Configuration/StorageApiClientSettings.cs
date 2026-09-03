using Microsoft.Extensions.Configuration;

namespace RECAMAS.Application.Configuration;

public class StorageApiClientSettings
{
    public required string BaseUrl { get; init; }

    public static StorageApiClientSettings BindFromConfiguration(IConfiguration configuration)
    {
        return new StorageApiClientSettings
        {
            BaseUrl = configuration["STORAGE_BASE_URL"]
                ?? throw new InvalidOperationException("STORAGE_BASE_URL is not configured."),
        };
    }
}
