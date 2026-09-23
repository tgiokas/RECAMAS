using ArsApi.Application.Interfaces;
using ArsApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Shared.Domain.Enums;

namespace ArsApi.WebApi.Controllers;

[ApiController]
[Route("api/v1/Alien/immigration-applicants")]
[Produces("application/json")]
public sealed class ArsContractController(IArsRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ArsEnvelope<IReadOnlyList<ArsPerson>>>> List(CancellationToken ct)
    {
        var records = await repository.SearchAsync(null, null, null, null, null, null, null, ct);
        return Ok(ArsEnvelope<IReadOnlyList<ArsPerson>>.Success(records.Select(Map).ToList()));
    }

    [HttpGet("{docNumber}")]
    public async Task<ActionResult<ArsEnvelope<ArsPerson>>> GetByDocument(string docNumber, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(docNumber))
            return BadRequest(ArsEnvelope<ArsPerson>.Failure("INVALID_PIN", "Document number is required."));

        var records = await repository.SearchAsync(null, null, null, null, docNumber, null, null, ct);
        var record = records.FirstOrDefault(x => string.Equals(x.PassportNo, docNumber, StringComparison.OrdinalIgnoreCase));
        if (record is null)
            return NotFound(ArsEnvelope<ArsPerson>.Failure("NO_DATA_FOUND", "No applicant was found."));

        return Ok(ArsEnvelope<ArsPerson>.Success(Map(record)));
    }

    private static ArsPerson Map(ArsRecord record) => new(
        new(record.LastName, record.FirstName, record.Nationality.ToAlpha3(), null,
            record.Gender switch { Gender.Male => "MALE", Gender.Female => "FEMALE", _ => "OTHER" },
            DateOnly.FromDateTime(record.DateOfBirth).ToString("yyyy-MM-dd")),
        new(record.PassportNo, "P", record.Nationality.ToAlpha3()));
}

public sealed record ArsEnvelope<T>(string ErrorCode, string? ErrorMessage, T? Data, bool Succeeded)
{
    public static ArsEnvelope<T> Success(T data) => new("SUCCESS", null, data, true);
    public static ArsEnvelope<T> Failure(string code, string message) => new(code, message, default, false);
}

public sealed record ArsPerson(ArsApplicantIdentity ApplicantIdentity, ArsTravelDocument TravelDocument);
public sealed record ArsApplicantIdentity(string Surname, string FirstName, string Nationality,
    string? OtherNationality, string Gender, string Dob);
public sealed record ArsTravelDocument(string? DocNo, string DocType, string IssuingCountry);
