using System.Diagnostics;
using System.Net;
using Microsoft.Extensions.Logging;
using RECAMAS.Infrastructure.Helpers.Redaction;

namespace RECAMAS.Infrastructure.ApiClients;

/// <summary>
/// Shared base for every outbound API client (reused platform services and the
/// external government systems alike). Ported from CivilianPortal's
/// Infrastructure.ApiClients.ApiClientBase — ADR: every concrete client calls
/// <see cref="SendRequestAsync"/> instead of _httpClient.SendAsync directly, so
/// request/response logging, redaction, and transport-failure handling live in
/// exactly one place rather than being reimplemented per client.
///
/// Polly retry (transient-fault) is configured separately at HttpClient
/// registration time in InfrastructureServiceRegistration — this class is the
/// last thing in the pipeline, closest to the actual send.
/// </summary>
public abstract class ApiClientBase
{
    protected readonly HttpClient _httpClient;
    protected readonly ILogger _logger;

    private const int MaxPayloadLength = 4096;

    private const string LogMessageTemplate =
        "HTTP {Direction} {RequestMethod} {RequestPath} {RequestPayload} responded {HttpStatusCode} {ResponsePayload} in {Elapsed:0.0000} ms";

    private const string ErrorMessageTemplate =
        "ERROR {Direction} {RequestMethod} {RequestPath} {RequestPayload} responded {HttpStatusCode} {ResponsePayload}";

    protected ApiClientBase(HttpClient httpClient, ILogger logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Sends the request and always returns an HttpResponseMessage — a transport-level
    /// exception (DNS failure, connection refused, timeout) is caught and turned into a
    /// synthetic 503 instead of propagating, so every caller has one code path
    /// (check IsSuccessStatusCode) instead of two (catch + check).
    /// </summary>
    protected async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
    {
        string requestBody = await BuildSafeRequestBodyAsync(request, cancellationToken);
        requestBody = Truncate(requestBody, MaxPayloadLength);

        var sw = Stopwatch.StartNew();

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.SendAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ErrorMessageTemplate, "Outgoing", request.Method,
                request.RequestUri, requestBody, HttpStatusCode.ServiceUnavailable, "");

            return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
            {
                Content = new StringContent("Service is temporarily unavailable."),
            };
        }

        sw.Stop();

        string responseBody = await BuildSafeResponseBodyAsync(response, cancellationToken);
        responseBody = Truncate(responseBody, MaxPayloadLength);

        int statusCode = (int)response.StatusCode;
        LogLevel logLevel = statusCode > 499 ? LogLevel.Error : LogLevel.Information;

        _logger.Log(logLevel, LogMessageTemplate, "Outgoing", request.Method,
            request.RequestUri, requestBody, statusCode, responseBody, sw.ElapsedMilliseconds);

        return response;
    }

    private static async Task<string> BuildSafeRequestBodyAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Content is null)
        {
            return string.Empty;
        }

        var contentType = request.Content.Headers.ContentType?.MediaType ?? string.Empty;

        // File uploads: never read the bytes into a string.
        if (contentType.StartsWith("multipart/", StringComparison.OrdinalIgnoreCase))
        {
            return $"[{contentType}]";
        }

        var raw = await request.Content.ReadAsStringAsync(cancellationToken);

        if (contentType.Equals("application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase))
        {
            return FormUrlEncodedRedactor.TryRedact(raw);
        }

        // JSON, SOAP/XML, and any other text body: best-effort JSON redaction,
        // falls through unredacted for non-JSON text (e.g. CASS SOAP envelopes)
        // since JsonRedactor.TryRedact returns the input unchanged when it
        // doesn't parse as JSON.
        return JsonRedactor.TryRedact(raw);
    }

    private static async Task<string> BuildSafeResponseBodyAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var contentType = response.Content.Headers.ContentType?.MediaType ?? string.Empty;

        bool isTextual =
            contentType.Length == 0 ||
            contentType.Contains("application/json", StringComparison.OrdinalIgnoreCase) ||
            contentType.StartsWith("text/", StringComparison.OrdinalIgnoreCase) ||
            contentType.Contains("xml", StringComparison.OrdinalIgnoreCase);

        if (!isTextual)
        {
            var len = response.Content.Headers.ContentLength;
            return $"[{contentType}; {len?.ToString() ?? "?"} bytes - body not logged]";
        }

        var raw = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonRedactor.TryRedact(raw);
    }

    private static string Truncate(string input, int maxLen)
    {
        if (string.IsNullOrEmpty(input) || input.Length <= maxLen)
        {
            return input;
        }

        return input.Substring(0, maxLen) + "...(truncated)";
    }
}
