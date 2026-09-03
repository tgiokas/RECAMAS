using RECAMAS.Application.Interfaces;

namespace RECAMAS.Infrastructure.ExternalClients;

/// Deliberately not wired to an HttpClient — see IFarClient remarks.
public class FarApiClient : IFarApiClient
{
}
