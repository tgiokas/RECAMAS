using CassApi.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Enums;

namespace CassApi.WebApi.Controllers;

// Provisional UAT-only contract. Replace with the generated SOAP/WSDL endpoint when supplied.
[ApiController]
[Route("uat/v1/cass")]
[Produces("application/json")]
public sealed class CassUatContractController(CassDbContext database) : ControllerBase
{
    [HttpPost("search")]
    public async Task<ActionResult<CassUatPerson?>> Search(CassUatPerson request, CancellationToken ct)
    {
        var query = database.CassRecords.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(request.Arc)) query = query.Where(x => x.Arc == request.Arc);
        else if (!string.IsNullOrWhiteSpace(request.PassportNumber))
            query = query.Where(x => x.PassportNo == request.PassportNumber);
        else
        {
            if (!string.IsNullOrWhiteSpace(request.FirstName)) query = query.Where(x => x.FirstName.ToLower() == request.FirstName.ToLower());
            if (!string.IsNullOrWhiteSpace(request.LastName)) query = query.Where(x => x.LastName.ToLower() == request.LastName.ToLower());
            if (request.DateOfBirth.HasValue)
            {
                var date = request.DateOfBirth.Value.ToDateTime(TimeOnly.MinValue);
                query = query.Where(x => x.DateOfBirth.Date == date.Date);
            }
        }

        var record = await query.FirstOrDefaultAsync(ct);
        if (record is null) return Ok(null);

        return Ok(new CassUatPerson(
            "CASS", record.Arc, request.ArsFolderNumber, record.CassFileNo,
            record.FirstName, record.LastName, DateOnly.FromDateTime(record.DateOfBirth),
            record.Nationality.ToAlpha3(), record.PassportNo,
            new Dictionary<string, object?>
            {
                ["ipStatus"] = record.IpStatusDecision?.ToString(),
                ["ipStatusType"] = record.IpStatusType?.ToString(),
                ["provisionalContract"] = true
            }));
    }
}

public sealed record CassUatPerson(string Source, string? Arc, string? ArsFolderNumber, string? CassFileNo,
    string? FirstName, string? LastName, DateOnly? DateOfBirth, string? NationalityCode,
    string? PassportNumber, Dictionary<string, object?> Extra);
