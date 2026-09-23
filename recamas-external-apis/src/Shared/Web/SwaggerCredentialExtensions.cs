using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Shared.Web;

public static class SwaggerCredentialExtensions
{
    public static void AddApiCredentialSecurity(
        this SwaggerGenOptions options,
        IConfiguration configuration)
    {
        var requirements = new OpenApiSecurityRequirement();

        AddHeaderScheme(
            options,
            requirements,
            "ApiKey",
            configuration["Security:ApiKeyHeader"] ?? "api-key",
            "API key required by this mock API.");

        if (configuration.GetValue("Security:RequireClientId", false))
        {
            AddHeaderScheme(
                options,
                requirements,
                "ClientId",
                "X-Client-ID",
                "Client identifier required by this mock API.");
        }

        if (configuration.GetValue("Security:RequireCorrelationId", false))
        {
            AddHeaderScheme(
                options,
                requirements,
                "CorrelationId",
                "X-Correlation-ID",
                "Correlation identifier for the request (for example, a UUID).");
        }

        options.AddSecurityRequirement(requirements);
    }

    private static void AddHeaderScheme(
        SwaggerGenOptions options,
        OpenApiSecurityRequirement requirements,
        string schemeId,
        string headerName,
        string description)
    {
        options.AddSecurityDefinition(schemeId, new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.ApiKey,
            In = ParameterLocation.Header,
            Name = headerName,
            Description = description
        });

        requirements.Add(new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = schemeId
            }
        }, Array.Empty<string>());
    }
}
