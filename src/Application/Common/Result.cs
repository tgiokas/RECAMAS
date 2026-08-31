namespace RECAMAS.Application.Common;

/// Standard return type for every Application service method, across every module.
/// Controllers check Success and map to an HTTP response — never throw raw exceptions
/// for expected business failures (see Errors/ErrorCodes.cs and ErrorCatalogExtensions).
public class Result<T>
{
    public bool Success { get; protected init; }
    public string? Message { get; protected init; }
    public string? ErrorCode { get; protected init; }
    public T? Data { get; set; }

    protected Result() { }

    public static Result<T> Ok(T data, string? message = null) => new()
    {
        Success = true,
        Data = data,
        Message = message,
    };

    public static Result<T> Fail(string message, string errorCode) => new()
    {
        Success = false,
        Message = message,
        ErrorCode = errorCode,
    };
}
