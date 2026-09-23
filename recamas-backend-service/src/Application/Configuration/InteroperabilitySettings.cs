using Microsoft.Extensions.Configuration;

namespace RECAMAS.Application.Configuration;

public sealed class ArsInteroperabilitySettings
{
    public required string BaseUrl { get; init; }
    public required string ApiKey { get; init; }

    public static ArsInteroperabilitySettings BindFromConfiguration(IConfiguration configuration) => new()
    {
        BaseUrl = configuration["ARS_BASE_URL"]
            ?? configuration["CYCONNECT_BASE_URL"]
            ?? throw new InvalidOperationException("ARS_BASE_URL or CYCONNECT_BASE_URL is not configured."),
        ApiKey = configuration["ARS_API_KEY"]
            ?? throw new InvalidOperationException("ARS_API_KEY is not configured."),
    };
}

public sealed class PoliceInteroperabilitySettings
{
    public required string BaseUrl { get; init; }
    public required string ApiKey { get; init; }
    public required string ClientId { get; init; }

    public static PoliceInteroperabilitySettings BindFromConfiguration(IConfiguration configuration) => new()
    {
        BaseUrl = configuration["POLICE_BASE_URL"]
            ?? configuration["STOPLIST_BASE_URL"]
            ?? configuration["ARRIVALS_DEPARTURES_BASE_URL"]
            ?? throw new InvalidOperationException("POLICE_BASE_URL is not configured."),
        ApiKey = configuration["POLICE_API_KEY"]
            ?? throw new InvalidOperationException("POLICE_API_KEY is not configured."),
        ClientId = configuration["POLICE_CLIENT_ID"]
            ?? throw new InvalidOperationException("POLICE_CLIENT_ID is not configured."),
    };
}

public sealed class CassInteroperabilitySettings
{
    public bool UseMock { get; init; }

    public static CassInteroperabilitySettings BindFromConfiguration(IConfiguration configuration) => new()
    {
        UseMock = bool.TryParse(configuration["CASS_USE_MOCK"], out var useMock) && useMock,
    };
}
