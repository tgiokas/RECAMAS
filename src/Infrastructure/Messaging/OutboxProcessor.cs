using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RECAMAS.Domain.Interfaces;

namespace RECAMAS.Infrastructure.Messaging;

/// <summary>
/// Polls OutboxMessage for pending rows and publishes each to Kafka, closing
/// the loop the transactional-outbox pattern needs (both EntityChangeAuditInterceptor
/// and OutboxDomainEventPublisher only ever write to the outbox table; this is
/// the only thing that ever calls Kafka). Modeled on CivilianPortal's own
/// OutboxProcessor, including its retry-cap behavior — a message that exhausts
/// MaxAttempts stops being retried and is logged at Error for manual follow-up
/// (a real dead-letter table/topic is future work, not built here, but "stop
/// retrying forever" — the prototype's flagged gap — is fixed).
///
/// Topic is a placeholder — see IDomainEventPublisher's own open item.
/// </summary>
public class OutboxProcessor : BackgroundService
{
    private const string PlaceholderTopic = "recamas.domain.events"; // TODO: confirm real topic name(s)
    private const int BatchSize = 20;
    private const int MaxAttempts = 5;
    private static readonly TimeSpan PollingInterval = TimeSpan.FromSeconds(5);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IProducer<string, string> _producer;
    private readonly ILogger<OutboxProcessor> _logger;

    public OutboxProcessor(IServiceScopeFactory scopeFactory, IProducer<string, string> producer, ILogger<OutboxProcessor> logger)
    {
        _scopeFactory = scopeFactory;
        _producer = producer;
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
                var kafkaMessage = new Message<string, string>
                {
                    Key = message.Key ?? message.EventId.ToString(),
                    Value = message.Payload,
                    Headers = new Headers
                    {
                        { "x-event-id", System.Text.Encoding.UTF8.GetBytes(message.EventId.ToString()) },
                        { "x-event-type", System.Text.Encoding.UTF8.GetBytes(message.EventType) },
                    },
                };

                await _producer.ProduceAsync(PlaceholderTopic, kafkaMessage, ct);
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
