using Microsoft.Extensions.Configuration;

namespace RECAMAS.Application.Configuration;

/// Study 9.4 gives a live request/response field table for this interface, but
/// 12.3.6 says the real integration is batch (asynchronous) file exchange over
/// the Police Public Zone — a direct contradiction, unresolved as of this
/// writing (see IArrivalsDeparturesClient remarks). BaseUrl here backs the
/// current provisional HTTP-based implementation only.
public class ArrivalsDeparturesClientSettings
{
    public required string BaseUrl { get; init; }

    public static ArrivalsDeparturesClientSettings BindFromConfiguration(IConfiguration configuration)
    {
        return new ArrivalsDeparturesClientSettings
        {
            BaseUrl = configuration["Services:ArrivalsDepartures:BaseUrl"]
                ?? throw new InvalidOperationException("Services:ArrivalsDepartures:BaseUrl is not configured."),
        };
    }
}
