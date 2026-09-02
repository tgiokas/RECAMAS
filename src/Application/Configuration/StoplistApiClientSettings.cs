using Microsoft.Extensions.Configuration;

namespace RECAMAS.Application.Configuration;

public class StoplistApiClientSettings
{
    public required string BaseUrl { get; init; }

    public static StoplistApiClientSettings BindFromConfiguration(IConfiguration configuration)
    {
        return new StoplistApiClientSettings
        {
            BaseUrl = configuration["STOPLIST_BASE_URL"]
                ?? throw new InvalidOperationException("STOPLIST_BASE_URL is not configured."),
        };
    }
}
