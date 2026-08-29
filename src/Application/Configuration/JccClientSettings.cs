using Microsoft.Extensions.Configuration;

namespace RECAMAS.Application.Configuration;

/// Study 9.6: JCC Trust Services SigningHub REST API. OAuth2 client-credentials
/// for an application-level token, then a user-scoped token for the signer.
/// "The detailed integration mechanism (API credentials, package/workflow
/// configuration, field placement, callback handling) shall be defined in
/// cooperation with JCC during implementation" — so this is a placeholder
/// shape, not a confirmed contract.
public class JccClientSettings
{
    public required string BaseUrl { get; init; }
    public string? ClientId { get; init; }
    public string? ClientSecret { get; init; }
    public string? CallbackUrl { get; init; }

    public static JccClientSettings BindFromConfiguration(IConfiguration configuration)
    {
        return new JccClientSettings
        {
            BaseUrl = configuration["Services:Jcc:BaseUrl"]
                ?? throw new InvalidOperationException("Services:Jcc:BaseUrl is not configured."),
            ClientId = configuration["Services:Jcc:ClientId"],
            ClientSecret = configuration["Services:Jcc:ClientSecret"],
            CallbackUrl = configuration["Services:Jcc:CallbackUrl"],
        };
    }
}
