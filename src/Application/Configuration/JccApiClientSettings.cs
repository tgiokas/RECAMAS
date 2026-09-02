using Microsoft.Extensions.Configuration;

namespace RECAMAS.Application.Configuration;

/// Specs 9.6: JCC Trust Services SigningHub REST API. OAuth2 client-credentials
/// for an application-level token, then a user-scoped token for the signer.
/// "The detailed integration mechanism (API credentials, package/workflow
/// configuration, field placement, callback handling) shall be defined in
/// cooperation with JCC during implementation" — so this is a placeholder
/// shape, not a confirmed contract.
public class JccApiClientSettings
{
    public required string BaseUrl { get; init; }
    public string? ClientId { get; init; }
    public string? ClientSecret { get; init; }
    public string? CallbackUrl { get; init; }

    public static JccApiClientSettings BindFromConfiguration(IConfiguration configuration)
    {
        return new JccApiClientSettings
        {
            BaseUrl = configuration["JCC_BASE_URL"]
                ?? throw new InvalidOperationException("JCC_BASE_URL is not configured."),
            ClientId = configuration["JCC_CLIENT_ID"],
            ClientSecret = configuration["JCC_CLIENT_SECRET"],
            CallbackUrl = configuration["JCC_CALLBACK_URL"],
        };
    }
}
