using RECAMAS.Application.Dtos.ExternalClients;

namespace RECAMAS.Application.Interfaces;

/// CASS (Cyprus Asylum Service's international-protection system). Specs
/// 12.3.4: synchronous SOAP web-service API consumed via the CY Connect
/// gateway. The interface stays request/response DTOs in, DTOs out — the
/// concrete client is responsible for building/parsing the actual SOAP
/// envelope, so callers never see SOAP vs. the ARS/REST shape underneath.
public interface ICassApiClient
{
    Task<CassSearchResult?> SearchAsync(CassSearchRequest request, CancellationToken ct = default);
}
