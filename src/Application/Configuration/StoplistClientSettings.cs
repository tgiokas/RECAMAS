using Microsoft.Extensions.Configuration;

namespace RECAMAS.Application.Configuration;

/// Same 9.5-vs-12.3.7 live-API-vs-batch-file contradiction as Arrivals/Departures — see IStoplistClient remarks.
public class StoplistClientSettings
{
    public required string BaseUrl { get; init; }

    public static StoplistClientSettings BindFromConfiguration(IConfiguration configuration)
    {
        return new StoplistClientSettings
        {
            BaseUrl = configuration["Services:Stoplist:BaseUrl"]
                ?? throw new InvalidOperationException("Services:Stoplist:BaseUrl is not configured."),
        };
    }
}
