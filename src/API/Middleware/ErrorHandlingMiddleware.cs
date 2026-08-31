using System.Net;
using System.Text.Json;

using RECAMAS.Application.Common;
using RECAMAS.Application.Errors;

namespace RECAMAS.Api.Middleware;

/// Catches any unhandled exception and returns a generic RECAMAS-000 result
/// Never rely on this for expected business failures; those should already
/// be a failed Result returned normally by the service, not an exception.
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, ILogger<ErrorHandlingMiddleware> logger)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        if (context.Response.HasStarted)
            return Task.CompletedTask;

        context.Response.Clear();
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var result = Result<string>.Fail(errorCode: ErrorCodes.Common.UnhandledException, message: "An unexpected error occurred");
        result.Data = exception.Message;

        var json = JsonSerializer.Serialize(result);
        return context.Response.WriteAsync(json);
    }
}

