using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RECAMAS.Application.Interfaces;
using RECAMAS.Domain.Interfaces;

namespace RECAMAS.Infrastructure.Messaging;

/// Polls OutboxMessage for pending rows and publishes each to Kafka, closing
/// the loop the transactional-outbox pattern needs (both EntityChangeAuditInterceptor
/// and OutboxDomainEventPublisher only ever write to the outbox table; this is
/// the only thing that ever calls Kafka). Modeled directly on CivilianPortal's
/// own OutboxProcessor — same IMessagePublisher dependency, same per-message
/// error handling, same route-by-EventType routing — rather than holding a
/// bare IProducer&lt;string, string&gt; the way this class used to.
///
/// Two deliberate differences from CivilianPortal's version, kept because
/// they're strict improvements and don't affect the shared KafkaPublisher/
/// IMessagePublisher contract: GetPendingAsync filters out already-exhausted
/// messages at the query level (maxAttempts passed straight to SQL) rather
/// than only checking retry count after picking a message up, and IDs are
/// long rather than int.
///
/// Topic naming is provisional — see IDomainEventPublisher's own open item.
public class OutboxProcessor : BackgroundService
{
    private const int BatchSize = 20;
    private const int MaxAttempts = 5;
    private static readonly TimeSpan PollingInterval = TimeSpan.FromSeconds(5);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<OutboxProcessor> _logger;

    public OutboxProcessor(IServiceScopeFactory scopeFactory, IMessagePublisher publisher, ILogger<OutboxProcessor> logger)
    {
        _scopeFactory = scopeFactory;
        _publisher = publisher;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("OutboxProcessor started, polling every {Interval}s", PollingInterval.TotalSeconds);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessPendingAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected OutboxProcessor error");
            }

            await Task.Delay(PollingInterval, stoppingToken);
        }
    }

    private async Task ProcessPendingAsync(CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var outboxRepository = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();

        var pending = await outboxRepository.GetPendingAsync(BatchSize, MaxAttempts, ct);
        if (pending.Count == 0)
        {
            return;
        }

        foreach (var message in pending)
        {
            try
            {
                var headers = new[]
                {
                    new KeyValuePair<string, string>("content-type", "application/json"),
                    new KeyValuePair<string, string>("x-event-id", message.EventId.ToString()),
                    new KeyValuePair<string, string>("x-event-type", message.EventType),
                };

                await _publisher.PublishRawJsonAsync(
                    route: message.EventType,
                    key: message.Key ?? message.EventId.ToString(),
                    jsonPayload: message.Payload,
                    headers: headers,
                    cancellationToken: ct);

                await outboxRepository.MarkAsProcessedAsync(message.Id, ct);
            }
            catch (Exception ex)
            {
                await outboxRepository.MarkAsFailedAsync(message.Id, ex.Message, ct);

                var attemptsSoFar = message.AttemptCount + 1;
                if (attemptsSoFar >= MaxAttempts)
                {
                    _logger.LogError(ex,
                        "Outbox message {EventId} ({EventType}, key={Key}) exhausted all {MaxAttempts} attempts — no longer retried, manual follow-up required",
                        message.EventId, message.EventType, message.Key, MaxAttempts);
                }
                else
                {
                    _logger.LogWarning(ex, "Failed to publish outbox message {EventId} (attempt {Attempt}/{Max})",
                        message.EventId, attemptsSoFar, MaxAttempts);
                }
            }
        }
    }
}
