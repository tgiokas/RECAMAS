using System.Net;
using System.Text.Json;
using RECAMAS.Application.Dtos.Interoperability.Shared;
using RECAMAS.Application.Errors;
using RECAMAS.Application.Interfaces.Interoperability;

namespace RECAMAS.Application.Modules.Interoperability;

public sealed class ExternalServiceDispatcher : IInteroperabilityService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly IReadOnlyDictionary<string, IExternalServiceAdapter> _adapters;

    public ExternalServiceDispatcher(IEnumerable<IExternalServiceAdapter> adapters)
    {
        var adapterList = adapters.ToList();
        var duplicate = adapterList.GroupBy(x => x.ServiceName, StringComparer.OrdinalIgnoreCase)
            .FirstOrDefault(x => x.Count() > 1);
        if (duplicate is not null)
            throw new InvalidOperationException($"Multiple interoperability adapters are registered for '{duplicate.Key}'.");

        _adapters = adapterList.ToDictionary(x => x.ServiceName, StringComparer.OrdinalIgnoreCase);
    }

    public async Task<ExternalServiceResult<TResponse>> ExecuteAsync<TResponse>(
        string serviceName,
        string operation,
        object request,
        ExternalRequestContext context,
        CancellationToken cancellationToken = default)
    {
        if (!_adapters.TryGetValue(serviceName, out var adapter))
            return Failure<TResponse>(serviceName, operation, context, ErrorCodes.Interoperability.ServiceNotFound, "Unknown external service.");

        try
        {
            var requestData = JsonSerializer.SerializeToElement(request, JsonOptions);
            var data = await /*_audit.ExecuteAsync(
                serviceName,
                operation,
                request,
                context,
                () =>*/ adapter.ExecuteAsync(operation, requestData, context, cancellationToken)/*,
                cancellationToken)*/;

            var typedData = data.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined
                ? default
                : data.Deserialize<TResponse>(JsonOptions);
            return new(true, serviceName, operation, typedData, context.CorrelationId, StatusCode: (int)HttpStatusCode.OK);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (NotSupportedException)
        {
            return Failure<TResponse>(serviceName, operation, context, ErrorCodes.Interoperability.OperationNotSupported, "The requested operation is not supported.");
        }
        catch (ArgumentException ex)
        {
            return Failure<TResponse>(serviceName, operation, context, ErrorCodes.Interoperability.InvalidRequest, ex.Message, (int)HttpStatusCode.BadRequest);
        }
        catch (TaskCanceledException)
        {
            return Failure<TResponse>(serviceName, operation, context, ErrorCodes.Interoperability.Timeout, "The external service timed out.", (int)HttpStatusCode.GatewayTimeout);
        }
        catch (HttpRequestException ex)
        {
            return Failure<TResponse>(serviceName, operation, context, ErrorCodes.Interoperability.Unavailable, "The external service is unavailable.", (int?)ex.StatusCode ?? (int)HttpStatusCode.BadGateway);
        }
        catch (JsonException)
        {
            return Failure<TResponse>(serviceName, operation, context, ErrorCodes.Interoperability.InvalidResponse, "The external service returned an invalid response.");
        }
        catch (InvalidOperationException)
        {
            return Failure<TResponse>(serviceName, operation, context, ErrorCodes.Interoperability.InvalidResponse, "The external service returned an unusable response.");
        }
    }

    private static ExternalServiceResult<T> Failure<T>(
        string service,
        string operation,
        ExternalRequestContext context,
        string code,
        string message,
        int statusCode = (int)HttpStatusCode.BadGateway) =>
        new(false, service, operation, default, context.CorrelationId, code, message, statusCode);
}
