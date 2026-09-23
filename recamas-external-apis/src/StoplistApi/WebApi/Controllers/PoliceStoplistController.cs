using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Enums;
using Shared.Web.Contracts;
using StoplistApi.Infrastructure.Data;

namespace StoplistApi.WebApi.Controllers;

[ApiController]
[Route("police/police-checks/v1/stoplist")]
[Produces("application/json")]
public sealed class PoliceStoplistController(StoplistDbContext database) : ControllerBase
{
    [HttpPost("search")]
    public async Task<ActionResult<StoplistResponse>> Search(StoplistSearchCriteria request, CancellationToken ct)
    {
        var validationError = PoliceContractValidation.Validate(
            request.DateOfBirth, request.Nationality, request.TravelDocumentType, request.Gender);
        if (validationError is not null) return BadRequest(new { error = validationError });

        Nationality? nationality = null;
        if (!string.IsNullOrWhiteSpace(request.Nationality))
        {
            if (!NationalityCodes.TryParseAlpha3(request.Nationality, out var parsed))
                return BadRequest(new { error = "nationality is not supported by this UAT dataset." });
            nationality = parsed;
        }

        var query = database.StoplistRecords.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(request.FirstNames))
            query = query.Where(x => x.FirstName.ToLower() == request.FirstNames.ToLower());
        if (!string.IsNullOrWhiteSpace(request.LastName))
            query = query.Where(x => x.LastName.ToLower() == request.LastName.ToLower());
        if (nationality.HasValue) query = query.Where(x => x.Nationality == nationality.Value);
        if (!string.IsNullOrWhiteSpace(request.TravelDocumentNumber))
            query = query.Where(x => x.PassportNo != null && x.PassportNo.ToLower() == request.TravelDocumentNumber.ToLower());
        query = ApplyDateFilter(query, request.DateOfBirth);

        return Ok(new StoplistResponse(await query.AnyAsync(x => x.IsOnStoplist, ct)));
    }

    private static IQueryable<StoplistApi.Domain.Entities.StoplistRecord> ApplyDateFilter(
        IQueryable<StoplistApi.Domain.Entities.StoplistRecord> query, DateOfBirthCriteria? dob)
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
