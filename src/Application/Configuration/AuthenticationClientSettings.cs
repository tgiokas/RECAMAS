using Microsoft.Extensions.Configuration;

namespace RECAMAS.Application.Configuration;

public class AuthenticationClientSettings
{
    public required string BaseUrl { get; init; }

    public static AuthenticationClientSettings BindFromConfiguration(IConfiguration configuration)
    {
        return new AuthenticationClientSettings
        {
            BaseUrl = configuration["Services:Authentication:BaseUrl"]
                ?? throw new InvalidOperationException("Services:Authentication:BaseUrl is not configured."),
        };
    }
}
