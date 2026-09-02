using Microsoft.Extensions.Configuration;

namespace RECAMAS.Application.Configuration;

/// Same 9.5-vs-12.3.7 live-API-vs-batch-file contradiction as Arrivals/Departures — see IStoplistApiClient remarks.
public class StoplistClientSettings
{
    public required string BaseUrl { get; init; }

    public static StoplistClientSettings BindFromConfiguration(IConfiguration configuration)
    {
        return new StoplistClientSettings
        {
            BaseUrl = configuration["STOPLIST_BASE_URL"]
                ?? throw new InvalidOperationException("STOPLIST_BASE_URL is not configured."),
        };
    }
}
