using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RECAMAS.Application.Interfaces;
using RECAMAS.Domain.Interfaces;

namespace RECAMAS.Infrastructure.Messaging;

/// Polls OutboxMessage for pending rows and publishes each to Kafka, closing
/// the loop the transactional-outbox pattern needs (both EntityChangeAuditInterceptor
/// and OutboxDomainEventPublisher only ever write to the outbox table; this is
/// the only thing that ever calls Kafka). Uses CivilianPortal's IMessagePublisher/
/// KafkaPublisher instead of holding a bare IProducer&lt;string, string&gt; directly,
/// but deliberately does NOT copy its route-by-EventType call: every RECAMAS
/// message publishes to one fixed topic instead, with EventType carried only
/// as a header. Reasoning (see architecture decision log): RECAMAS's own
/// EventType values are unbounded (a free-text "action" string, or
/// typeof(TEvent).Name for any future domain event class) rather than
/// CivilianPortal's single fixed, config-driven value — routing by EventType
/// here would mean a new topic gets implied every time a developer adds a new
/// action string or event class, with no catalog anywhere of what exists.
/// That risks silent outbox failures if the broker doesn't auto-create topics,
/// ungoverned topic sprawl if it does, and it breaks AuditLog's "subscribe once,
/// get everything for compliance" requirement. One topic keyed by
/// aggregate/correlation id also keeps one entity's full event history in a
/// single ordered stream, which per-type topics would not.
///
/// Two further deliberate differences from CivilianPortal's version, kept
/// because they're strict improvements and don't affect the shared
/// KafkaPublisher/IMessagePublisher contract: GetPendingAsync filters out
/// already-exhausted messages at the query level (maxAttempts passed straight
/// to SQL) rather than only checking retry count after picking a message up,
/// and IDs are long rather than int.
///
/// PlaceholderTopic's real name is still provisional — see IDomainEventPublisher's own open item.
public class OutboxProcessor : BackgroundService
{
    private const string PlaceholderTopic = "recamas.domain.events"; // TODO: confirm real topic name with AuditLog/Notifications owners
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
                    route: PlaceholderTopic,
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
