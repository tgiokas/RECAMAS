using ArrivalsApi.Domain.Entities;
using ArrivalsApi.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Enums;
using Shared.Web.Contracts;

namespace ArrivalsApi.WebApi.Controllers;

[ApiController]
[Route("police/police-checks/v1/arrivals-departures")]
[Produces("application/json")]
public sealed class PoliceArrivalsDeparturesController(ArrivalsDbContext database) : ControllerBase
{
    [HttpPost("search")]
    public async Task<ActionResult<ArrivalsDeparturesResponse>> Search(
        ArrivalsDeparturesSearchCriteria request, CancellationToken ct)
    {
        var validationError = PoliceContractValidation.Validate(request.DateOfBirth, request.Nationality);
        if (validationError is not null) return BadRequest(new { error = validationError });

        Nationality? nationality = null;
        if (!string.IsNullOrWhiteSpace(request.Nationality))
        {
            if (!NationalityCodes.TryParseAlpha3(request.Nationality, out var parsed))
                return BadRequest(new { error = "nationality is not supported by this UAT dataset." });
            nationality = parsed;
        }

        var query = database.ArrivalRecords.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(request.FirstNames))
            query = query.Where(x => x.FirstName.ToLower() == request.FirstNames.ToLower());
        if (!string.IsNullOrWhiteSpace(request.LastName))
            query = query.Where(x => x.LastName.ToLower() == request.LastName.ToLower());
        if (nationality.HasValue) query = query.Where(x => x.Nationality == nationality.Value);
        if (!string.IsNullOrWhiteSpace(request.TravelDocumentNumber))
            query = query.Where(x => x.PassportNo != null && x.PassportNo.ToLower() == request.TravelDocumentNumber.ToLower());
        query = ApplyDateFilter(query, request.DateOfBirth);

        var records = await query.OrderBy(x => x.MovementDate).ToListAsync(ct);
        var arrivals = records.Where(x => x.MovementType == MovementType.Arrival).Select(MapArrival).ToList();
        var departures = records.Where(x => x.MovementType == MovementType.Departure).Select(MapDeparture).ToList();
        return Ok(new ArrivalsDeparturesResponse(arrivals, departures));
    }

    private static ArrivalResponse MapArrival(ArrivalRecord record) => new(
        StableId(record), StablePersonId(record), DateOnly.FromDateTime(record.MovementDate),
        record.Nationality.ToAlpha3(), record.PassportNo, null, null, 0);

    private static DepartureResponse MapDeparture(ArrivalRecord record) => new(
        StableId(record), StablePersonId(record), DateOnly.FromDateTime(record.MovementDate),
        record.Nationality.ToAlpha3(), record.PassportNo, 0);

    private static long StableId(ArrivalRecord record) => BitConverter.ToInt64(record.Id.ToByteArray()) & long.MaxValue;
    private static long StablePersonId(ArrivalRecord record) =>
        Math.Abs(StringComparer.Ordinal.GetHashCode(record.Arc));

    private static IQueryable<ArrivalRecord> ApplyDateFilter(IQueryable<ArrivalRecord> query, DateOfBirthCriteria? dob)
    {
        if (dob?.ExactDate is { } exact)
        {
            var value = exact.ToDateTime(TimeOnly.MinValue);
            query = query.Where(x => x.DateOfBirth.Date == value.Date);
        }
        if (dob?.StartDate is { } start && dob.EndDate is { } end)
        {
            var from = start.ToDateTime(TimeOnly.MinValue);
            var to = end.ToDateTime(TimeOnly.MinValue);
            query = query.Where(x => x.DateOfBirth.Date >= from.Date && x.DateOfBirth.Date <= to.Date);
        }
        return query;
    }
}
