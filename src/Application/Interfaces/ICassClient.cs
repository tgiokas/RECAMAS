using RECAMAS.Application.Dtos.ExternalClients;

namespace RECAMAS.Application.Interfaces;

/// <summary>
/// CASS (Cyprus Asylum Service's international-protection system). Study
/// 12.3.4: synchronous SOAP web-service API consumed via the CY Connect
/// gateway. The interface stays request/response DTOs in, DTOs out — the
/// concrete client is responsible for building/parsing the actual SOAP
/// envelope, so callers never see SOAP vs. the ARS/REST shape underneath.
/// </summary>
public interface ICassClient
{
    Task<CassSearchResult?> SearchAsync(CassSearchRequest request, CancellationToken ct = default);
}
