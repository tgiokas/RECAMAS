using ArrivalsApi.Application.DTOs;
using ArrivalsApi.Application.Interfaces;
using ArrivalsApi.Domain.Entities;

namespace ArrivalsApi.Application.Services;

public class ArrivalsService(IArrivalsRepository repo)
{
    public async Task<IEnumerable<ArrivalMovementResponse>> SearchAsync(
        ArrivalsSearchRequest req, CancellationToken ct = default)
    {
        var records = await repo.SearchAsync(req.Arc, req.Name, req.Surname,
            req.Nationality, req.PassportNo, req.DateOfBirth, ct);
        return records.Select(r => new ArrivalMovementResponse(
            r.Arc, r.FirstName, r.LastName, r.MovementType, r.MovementDate, r.Airport));
    }

    public async Task<ArrivalMovementResponse> CreateAsync(
        ArrivalsCreateRequest req, CancellationToken ct = default)
    {
        var e = new ArrivalRecord {
            Arc = req.Arc, FirstName = req.FirstName, LastName = req.LastName,
            Nationality = req.Nationality, PassportNo = req.PassportNo,
            DateOfBirth = req.DateOfBirth, MovementType = req.MovementType,
            MovementDate = req.MovementDate, Airport = req.Airport
        };
        var created = await repo.CreateAsync(e, ct);
        return new ArrivalMovementResponse(created.Arc, created.FirstName,
            created.LastName, created.MovementType, created.MovementDate, created.Airport);
    }
}
