using Microsoft.Extensions.Configuration;

namespace RECAMAS.Application.Configuration;

/// <summary>Used for JWT Bearer validation in Program.cs — see its own remarks on the RequireHttpsMetadata/Authority relationship.</summary>
public class KeycloakSettings
{
    public required string Authority { get; init; }
    public required string Audience { get; init; }

    public static KeycloakSettings BindFromConfiguration(IConfiguration configuration)
    {
        return new KeycloakSettings
        {
            Authority = configuration["KEYCLOAK_AUTHORITY"]
                ?? throw new InvalidOperationException("KEYCLOAK_AUTHORITY is not configured."),
            Audience = configuration["KEYCLOAK_AUDIENCE"]
                ?? throw new InvalidOperationException("KEYCLOAK_AUDIENCE is not configured."),
        };
    }
}
