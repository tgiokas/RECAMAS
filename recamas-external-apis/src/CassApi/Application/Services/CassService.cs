using CassApi.Application.DTOs;
using CassApi.Application.Interfaces;
using CassApi.Domain.Entities;

namespace CassApi.Application.Services;

public class CassService(ICassRepository repo)
{
    public async Task<IEnumerable<CassSearchResponse>> SearchAsync(CassSearchRequest req, CancellationToken ct = default)
    {
        var records = await repo.SearchAsync(req.Arc, req.Name, req.Surname,
            req.Nationality, req.PassportNo, req.DateOfBirth, req.CassFileNo, ct);
        return records.Select(Map);
    }

    public async Task<CassSearchResponse> CreateAsync(CassCreateRequest req, CancellationToken ct = default)
    {
        var e = new CassRecord {
            Arc = req.Arc, FirstName = req.FirstName, LastName = req.LastName,
            Nationality = req.Nationality, PassportNo = req.PassportNo,
            PassportExpirationDate = req.PassportExpirationDate, DateOfBirth = req.DateOfBirth,
            Address = req.Address, PhoneNo = req.PhoneNo, CassFileNo = req.CassFileNo,
            IpStatusType = req.IpStatusType, IpStatusDateOfGranting = req.IpStatusDateOfGranting,
            IpStatusExpiryDate = req.IpStatusExpiryDate, IpStatusDecisionDate = req.IpStatusDecisionDate,
            IpStatusDecision = req.IpStatusDecision,
            IpApplicationType = req.IpApplicationType, IpApplicationSubmissionDate = req.IpApplicationSubmissionDate,
            IpApplicationExpiryDate = req.IpApplicationExpiryDate, IpApplicationDecisionDate = req.IpApplicationDecisionDate,
            IpApplicationDecision = req.IpApplicationDecision,
            AppealType = req.AppealType, AppealNumber = req.AppealNumber,
            AppealDate = req.AppealDate, AppealDecisionDate = req.AppealDecisionDate, AppealDecision = req.AppealDecision,
            ReturnDecisionDate = req.ReturnDecisionDate, ReturnDecisionText = req.ReturnDecisionText,
            TcnReceiptDate = req.TcnReceiptDate, VoluntaryReturnDeadlineDays = req.VoluntaryReturnDeadlineDays,
            EntryBanDurationMonths = req.EntryBanDurationMonths
        };
        return Map(await repo.CreateAsync(e, ct));
    }

    private static CassSearchResponse Map(CassRecord r) => new(
        r.Arc, r.PassportNo, r.PassportExpirationDate, r.Address, r.PhoneNo, r.CassFileNo,
        r.IpStatusType == null ? null : new CassIpStatusDto(r.IpStatusType, r.IpStatusDateOfGranting,
            r.IpStatusExpiryDate, r.IpStatusDecisionDate, r.IpStatusDecision),
        r.IpApplicationType == null ? null : new CassIpApplicationDto(r.IpApplicationType,
            r.IpApplicationSubmissionDate, r.IpApplicationExpiryDate, r.IpApplicationDecisionDate, r.IpApplicationDecision),
        r.AppealType == null ? null : new CassAppealDto(r.AppealType, r.AppealNumber,
            r.AppealDate, r.AppealDecisionDate, r.AppealDecision),
        r.ReturnDecisionDate == null ? null : new CassReturnDecisionDto(r.ReturnDecisionDate,
            r.ReturnDecisionText, r.TcnReceiptDate, r.VoluntaryReturnDeadlineDays, r.EntryBanDurationMonths)
    );
}
