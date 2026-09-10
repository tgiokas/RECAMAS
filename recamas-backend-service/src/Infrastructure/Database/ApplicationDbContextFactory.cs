using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Pgvector.EntityFrameworkCore;

namespace RECAMAS.Infrastructure.Database;

/// <summary>
/// Allows EF tooling to create the context without bootstrapping API integrations.
/// RECAMAS_DB_CONNECTION is used when present; the fallback only supplies model-time options.
/// </summary>
public sealed class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("RECAMAS_DB_CONNECTION")
            ?? "Host=localhost;Database=recamas-dev;Username=recamas-app;Password=dev_password_only";

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(connectionString, npgsql => npgsql.UseVector())
            .Options;

        return new ApplicationDbContext(options);
    }
}
