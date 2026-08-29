using Microsoft.Extensions.Configuration;

namespace RECAMAS.Application.Configuration;

public class StorageClientSettings
{
    public required string BaseUrl { get; init; }

    public static StorageClientSettings BindFromConfiguration(IConfiguration configuration)
    {
        return new StorageClientSettings
        {
            BaseUrl = configuration["Services:Storage:BaseUrl"]
                ?? throw new InvalidOperationException("Services:Storage:BaseUrl is not configured."),
        };
    }
}
