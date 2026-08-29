using System.Net;
using System.Text.Json;
using RECAMAS.Application.Common;
using RECAMAS.Application.Errors;

namespace RECAMAS.Api.Middleware;

/// <summary>
/// Catches any unhandled exception and returns a generic RECAMAS-000 result —
/// mirrors the Authentication service's ErrorHandlingMiddleware exactly.
/// Never rely on this for expected business failures; those should already
/// be a failed Result&lt;T&gt; returned normally by the service, not an exception.
/// </summary>
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception processing {Method} {Path}", context.Request.Method, context.Request.Path);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var result = Result.Fail("An unexpected error occurred.", ErrorCodes.Common.UnhandledException);
            await context.Response.WriteAsync(JsonSerializer.Serialize(result));
        }
    }
}
