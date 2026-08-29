using Microsoft.Extensions.Configuration;

namespace RECAMAS.Application.Configuration;

/// Study 12.3.3: ARS and CASS are both consumed through the single CY Connect
/// API gateway, not point-to-point — so one BaseUrl/credential set here backs
/// both ArsClient and CassClient, each hitting a different relative path.
///
/// Auth mechanism is genuinely unconfirmed: 12.3.2 allows "OAuth 2.0 and mutual
/// TLS" for the platform's OpenAPI capability in general, but doesn't say which
/// one CY Connect itself requires. ClientId/ClientSecret below assumes OAuth2
/// client-credentials as the default guess — flagged, not confirmed.
public class CyConnectSettings
{
    public required string BaseUrl { get; init; }
    public string? ClientId { get; init; }
    public string? ClientSecret { get; init; }

    public static CyConnectSettings BindFromConfiguration(IConfiguration configuration)
    {
        return new CyConnectSettings
        {
            BaseUrl = configuration["CYCONNECT_BASE_URL"]
                ?? throw new InvalidOperationException("CYCONNECT_BASE_URL is not configured."),
            ClientId = configuration["CYCONNECT_CLIENT_ID"],
            ClientSecret = configuration["CYCONNECT_CLIENT_SECRET"],
        };
    }
}
