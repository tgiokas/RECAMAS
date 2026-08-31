namespace RECAMAS.Application.Errors;

/// Loaded once at startup from errors.json (see ErrorCatalog.LoadFromFile,
/// wired up in InfrastructureServiceRegistration). The API fails fast at
/// startup if errors.json is missing or a referenced code has no entry
public interface IErrorCatalog
{
    string GetError(string errorCode);
    bool TryGetMessage(string errorCode, out string message);
}
