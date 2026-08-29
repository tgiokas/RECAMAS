using RECAMAS.Application.Common;

namespace RECAMAS.Application.Errors;

/// Lets a service write:
///   return _errors.Fail&lt;CaseDto&gt;(ErrorCodes.Case.InvalidStageTransition);
/// instead of hardcoding a message string inline. The message text always comes
/// from errors.json via IErrorCatalog — never pass a literal string as the message.
public static class ErrorCatalogExtensions
{
    public static Result<T> Fail<T>(this IErrorCatalog catalog, string errorCode)
    {
        var message = catalog.GetMessage(errorCode);
        return Result<T>.Fail(message, errorCode);
    }
}
