using System.Text;
using Microsoft.Extensions.Logging;
using RECAMAS.Application.Dtos.ExternalClients;
using RECAMAS.Application.Interfaces;
using RECAMAS.Infrastructure.ApiClients;

namespace RECAMAS.Infrastructure.ExternalClients;

/// CASS via the CY Connect gateway, consumed as a SOAP web service (Study
/// 12.3.4) — the one external system here that isn't plain JSON/REST.
///
/// The envelope built below is a structural placeholder, not a real CASS
/// contract: there is no WSDL available yet ("the precise API ... shall be
/// determined during system design"). Real work needed once the WSDL exists:
/// generate/hand-write the actual envelope shape, correct SOAPAction header,
/// and a real response parser in place of the not-implemented one below.
public class CassClient : ApiClientBase, ICassClient
{
    // TODO: confirm real CY Connect route + SOAPAction for CASS once the WSDL is available.
    private const string SearchEndpoint = "/cass/ws/tcn-search";

    public CassClient(HttpClient httpClient, ILogger<CassClient> logger)
        : base(httpClient, logger)
    {
    }

    public async Task<CassSearchResult?> SearchAsync(CassSearchRequest request, CancellationToken ct = default)
    {
        var envelope = BuildSearchEnvelope(request);

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, SearchEndpoint)
        {
            Content = new StringContent(envelope, Encoding.UTF8, "text/xml"),
        };

        var response = await SendRequestAsync(httpRequest, ct);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("CASS search returned {StatusCode} for ARC {Arc}", (int)response.StatusCode, request.Arc);
            return null;
        }

        var xml = await response.Content.ReadAsStringAsync(ct);
        return ParseSearchResponse(xml);
    }

    private static string BuildSearchEnvelope(CassSearchRequest request)
    {
        // Placeholder shape only — real element names/namespaces come from CASS's WSDL.
        return $"""
            <?xml version="1.0" encoding="utf-8"?>
            <soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
              <soap:Body>
                <TcnSearch>
                  <Arc>{request.Arc}</Arc>
                  <Name>{request.Name}</Name>
                  <Surname>{request.Surname}</Surname>
                  <Nationality>{request.Nationality}</Nationality>
                  <PassportNo>{request.PassportNo}</PassportNo>
                  <DateOfBirth>{request.DateOfBirth:yyyy-MM-dd}</DateOfBirth>
                  <CassFileNo>{request.CassFileNo}</CassFileNo>
                </TcnSearch>
              </soap:Body>
            </soap:Envelope>
            """;
    }

    private CassSearchResult? ParseSearchResponse(string xml)
    {
        // TODO: real SOAP response parsing once CASS's WSDL/response schema exists.
        _logger.LogWarning("CassClient.ParseSearchResponse is not yet implemented (no WSDL available)");
        return null;
    }
}
