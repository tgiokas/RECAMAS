using StoplistApi.Application.DTOs;
using StoplistApi.Application.Interfaces;
using StoplistApi.Domain.Entities;

namespace StoplistApi.Application.Services;

public class StoplistService(IStoplistRepository repo)
{
    public async Task<IEnumerable<StoplistSearchResponse>> SearchAsync(
        StoplistSearchRequest req, CancellationToken ct = default)
    {
        var records = await repo.SearchAsync(req.Arc, req.Name, req.Surname,
            req.Nationality, req.PassportNo, req.DateOfBirth, ct);
        return records.Select(Map);
    }

    public async Task<StoplistSearchResponse> CreateAsync(
        StoplistCreateRequest req, CancellationToken ct = default)
    {
        var e = new StoplistRecord
        {
            Arc = req.Arc, FirstName = req.FirstName, LastName = req.LastName,
            Nationality = req.Nationality, PassportNo = req.PassportNo,
            DateOfBirth = req.DateOfBirth, IsOnStoplist = req.IsOnStoplist,
            UniqueEntryBanNumber = req.UniqueEntryBanNumber,
            StoplistEntryDate = req.StoplistEntryDate
        };
        return Map(await repo.CreateAsync(e, ct));
    }

    private static StoplistSearchResponse Map(StoplistRecord r) =>
        new(r.Arc, r.FirstName, r.LastName,
            r.IsOnStoplist, r.UniqueEntryBanNumber, r.StoplistEntryDate);
}
