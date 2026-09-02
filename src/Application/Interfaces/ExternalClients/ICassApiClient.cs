using RECAMAS.Application.Dtos.ExternalClients;

namespace RECAMAS.Application.Interfaces;

/// CASS (Cyprus Asylum Service's international-protection system). 
/// Specs 12.3.4: synchronous SOAP web-service API consumed via the CY Connect
public interface ICassApiClient
{
    Task<CassSearchResult?> SearchAsync(CassSearchRequest request, CancellationToken ct = default);
}
