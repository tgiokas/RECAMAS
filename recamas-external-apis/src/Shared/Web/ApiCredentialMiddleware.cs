using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Shared.Web;

public sealed class ApiCredentialMiddleware(RequestDelegate next, IConfiguration configuration)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (IsAnonymousPath(context.Request.Path))
        {
            await next(context);
            return;
        }

        var apiKeyHeader = configuration["Security:ApiKeyHeader"] ?? "api-key";
        var expectedApiKey = configuration["Security:ApiKey"];
        var requireClientId = configuration.GetValue("Security:RequireClientId", false);
        var requireCorrelationId = configuration.GetValue("Security:RequireCorrelationId", false);

        if (string.IsNullOrWhiteSpace(expectedApiKey))
        {
            await WriteError(context, StatusCodes.Status500InternalServerError, "Mock API authentication is not configured.");
            return;
        }

        var suppliedApiKey = context.Request.Headers[apiKeyHeader].FirstOrDefault();
        if (!SecureEquals(suppliedApiKey, expectedApiKey))
        {
            await WriteError(context, StatusCodes.Status401Unauthorized, "Missing or invalid API key.");
            return;
        }

        if (requireClientId)
        {
            var expectedClientId = configuration["Security:ClientId"];
            var suppliedClientId = context.Request.Headers["X-Client-ID"].FirstOrDefault();
            if (!SecureEquals(suppliedClientId, expectedClientId))
            {
                await WriteError(context, StatusCodes.Status403Forbidden, "Client ID is not authorized.");
                return;
            }
        }

        var correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault();
        if (requireCorrelationId && string.IsNullOrWhiteSpace(correlationId))
        {
            await WriteError(context, StatusCodes.Status400BadRequest, "X-Correlation-ID is required.");
            return;
        }

        if (!string.IsNullOrWhiteSpace(correlationId))
            context.Response.Headers["X-Correlation-ID"] = correlationId;

        await next(context);
    }

    private static bool IsAnonymousPath(PathString path) =>
        path == "/" || path.StartsWithSegments("/swagger") || path.StartsWithSegments("/health");

    private static bool SecureEquals(string? supplied, string? expected)
    {
        if (string.IsNullOrEmpty(supplied) || string.IsNullOrEmpty(expected)) return false;
        var suppliedHash = SHA256.HashData(Encoding.UTF8.GetBytes(supplied));
        var expectedHash = SHA256.HashData(Encoding.UTF8.GetBytes(expected));
        return CryptographicOperations.FixedTimeEquals(suppliedHash, expectedHash);
    }

    private static Task WriteError(HttpContext context, int statusCode, string message)
    {
        context.Response.StatusCode = statusCode;
        return context.Response.WriteAsJsonAsync(new { error = message });
    }
}

public static class ApiCredentialApplicationBuilderExtensions
{
    public static IApplicationBuilder UseApiCredentials(this IApplicationBuilder app) =>
        app.UseMiddleware<ApiCredentialMiddleware>();
}
