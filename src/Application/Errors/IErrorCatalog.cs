namespace RECAMAS.Application.Errors;

/// Loaded once at startup from errors.json (see ErrorCatalog.LoadFromFile,
/// wired up in InfrastructureServiceRegistration). The API fails fast at
/// startup if errors.json is missing or a referenced code has no entry —
/// mirrors the Authentication service's convention exactly.
public interface IErrorCatalog
{
    string GetMessage(string errorCode);
    bool TryGetMessage(string errorCode, out string message);
}
