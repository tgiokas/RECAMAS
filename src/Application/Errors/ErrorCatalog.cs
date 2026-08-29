using System.Text.Json;

namespace RECAMAS.Application.Errors;

/// <summary>
/// Loads errors.json (flat {"RECAMAS-000": "message", ...} map, one entry per
/// ErrorCodes constant) once at startup. Ported from CivilianPortal's
/// ErrorCatalog, adapted to RECAMAS's IErrorCatalog shape (GetMessage/TryGetMessage
/// rather than CivilianPortal's ErrorInfo/GetError) and its flat JSON shape
/// rather than CivilianPortal's grouped-by-application-name array.
/// </summary>
public sealed class ErrorCatalog : IErrorCatalog
{
    private readonly Dictionary<string, string> _messagesByCode;

    private ErrorCatalog(Dictionary<string, string> messagesByCode)
    {
        _messagesByCode = messagesByCode;
    }

    public static IErrorCatalog LoadFromFile(string path)
    {
        var json = File.ReadAllText(path);
        var messagesByCode = JsonSerializer.Deserialize<Dictionary<string, string>>(json)
            ?? new Dictionary<string, string>();

        return new ErrorCatalog(new Dictionary<string, string>(messagesByCode, StringComparer.OrdinalIgnoreCase));
    }

    public string GetMessage(string errorCode)
    {
        return TryGetMessage(errorCode, out var message)
            ? message
            : $"Unknown error code '{errorCode}' - no entry in errors.json.";
    }

    public bool TryGetMessage(string errorCode, out string message)
    {
        if (_messagesByCode.TryGetValue(errorCode, out var found))
        {
            message = found;
            return true;
        }

        message = string.Empty;
        return false;
    }
}
