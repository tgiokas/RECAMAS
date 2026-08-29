using Microsoft.Extensions.Configuration;

namespace RECAMAS.Application.Configuration;

public class DatabaseSettings
{
    public required string ConnectionString { get; init; }

    public static DatabaseSettings BindFromConfiguration(IConfiguration configuration)
    {
        return new DatabaseSettings
        {
            ConnectionString = configuration["RECAMAS_DB_CONNECTION"]
                ?? throw new InvalidOperationException("RECAMAS_DB_CONNECTION is not configured."),
        };
    }
}
