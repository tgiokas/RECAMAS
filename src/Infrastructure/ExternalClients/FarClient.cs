using RECAMAS.Application.Interfaces;

namespace RECAMAS.Infrastructure.ExternalClients;

/// <summary>
/// Deliberately not wired to an HttpClient — see IFarClient remarks. Exists so
/// the module list / DI registration reflects "the FAR hook exists" the same
/// way INotificationClient does for Kafka-only Notifications, without
/// pretending there's a real endpoint to call yet.
/// </summary>
public class FarClient : IFarClient
{
}
