using RECAMAS.Application.Common;

namespace RECAMAS.Application.Errors;

public static class ErrorCatalogExtensions
{
    public static Result<T> Fail<T>(this IErrorCatalog catalog, string errorCode)
    {
        var message = catalog.GetError(errorCode);
        return Result<T>.Fail(message, errorCode);
    }
}
