using System.Text;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;
using RECAMAS.Application.Dtos.ExternalClients;
using RECAMAS.Application.Interfaces;
using RECAMAS.Infrastructure.ApiClients;

namespace RECAMAS.Infrastructure.ExternalClients;

/// CASS via the CY Connect gateway, consumed as a SOAP web service (Specs
/// 12.3.4) — the one external system here that isn't plain JSON/REST.
///
/// The envelope built below is a structural placeholder, not a real CASS
/// contract: there is no WSDL available yet 
public class CassApiClient : ApiClientBase, ICassApiClient
{
    // TODO: confirm real CY Connect route + SOAPAction for CASS once the WSDL is available.
    private const string SearchEndpoint = "/cass/ws/tcn-search";

    public CassApiClient(HttpClient httpClient, ILogger<CassApiClient> logger)
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
        // Built with XElement (not string interpolation) so field values are XML-escaped
        // rather than dropped into the envelope raw.
        XNamespace soap = "http://schemas.xmlsoap.org/soap/envelope/";

        var envelope = new XElement(soap + "Envelope",
            new XAttribute(XNamespace.Xmlns + "soap", soap),
            new XElement(soap + "Body",
                new XElement("TcnSearch",
                    new XElement("Arc", request.Arc),
                    new XElement("Name", request.Name),
                    new XElement("Surname", request.Surname),
                    new XElement("Nationality", request.Nationality),
                    new XElement("PassportNo", request.PassportNo),
                    new XElement("DateOfBirth", request.DateOfBirth?.ToString("yyyy-MM-dd")),
                    new XElement("CassFileNo", request.CassFileNo))));

        return new XDocument(new XDeclaration("1.0", "utf-8", null), envelope).ToString();
    }

    private CassSearchResult? ParseSearchResponse(string xml)
    {
        // TODO: real SOAP response parsing once CASS's WSDL/response schema exists.
        _logger.LogWarning("CassApiClient.ParseSearchResponse is not yet implemented (no WSDL available)");
        return null;
    }
}
