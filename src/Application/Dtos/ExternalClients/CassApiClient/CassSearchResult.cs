namespace RECAMAS.Application.Dtos.ExternalClients;

/// Specs Tables 158-162 (CASS Response Fields).
public sealed record CassSearchResult(
    CassTcnInformation TcnInformation,
    CassIpStatus? IpStatus,
    IReadOnlyList<CassIpApplication> IpApplications,
    IReadOnlyList<CassAppeal> Appeals,
    CassReturnDecision? ReturnDecision);
