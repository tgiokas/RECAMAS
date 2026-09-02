using Microsoft.Extensions.Configuration;

namespace RECAMAS.Application.Configuration;

/// Specs 9.4 gives a live request/response field table for this interface, but
public class ArrivalsDeparturesClientSettings
{
    public required string BaseUrl { get; init; }

    public static ArrivalsDeparturesClientSettings BindFromConfiguration(IConfiguration configuration)
    {
        return new ArrivalsDeparturesClientSettings
        {
            BaseUrl = configuration["ARRIVALS_DEPARTURES_BASE_URL"]
                ?? throw new InvalidOperationException("ARRIVALS_DEPARTURES_BASE_URL is not configured."),
        };
    }
}
