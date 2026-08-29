namespace RECAMAS.Application.Interfaces;

/// Same shape as CivilianPortal's IMessagePublisher, so every Kafka producer in
/// the codebase goes through one abstraction instead of each caller holding its
/// own IProducer&lt;string, string&gt;. OutboxProcessor is RECAMAS's only caller
/// today (PublishRawJsonAsync, since the outbox already stores a serialized
/// payload) — PublishJsonAsync&lt;T&gt; exists for parity and any future direct
/// (non-outbox) publish.
public interface IMessagePublisher
{
    Task PublishJsonAsync<T>(
        string route,
        string key,
        T payload,
        IEnumerable<KeyValuePair<string, string>>? headers = null,
        CancellationToken cancellationToken = default);

    Task PublishRawJsonAsync(
        string route,
        string key,
        string jsonPayload,
        IEnumerable<KeyValuePair<string, string>>? headers = null,
        CancellationToken cancellationToken = default);
}
