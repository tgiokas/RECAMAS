using CassApi.Application.DTOs;
using CassApi.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Domain.Enums;

namespace CassApi.WebApi.Controllers;

[ApiController]
[Route("api/cass")]
[Produces("application/json")]
public class CassController(CassService service) : ControllerBase
{
    /// <summary>Search CASS records — Table 157 request fields</summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(IEnumerable<CassSearchResponse>), 200)]
    public async Task<IActionResult> Search(
        [FromQuery] string? arc, [FromQuery] string? name, [FromQuery] string? surname,
        [FromQuery] Nationality? nationality, [FromQuery] string? passportNo,
        [FromQuery] DateTime? dateOfBirth, [FromQuery] string? cassFileNo,
        CancellationToken ct = default)
    {
        if (arc is null && name is null && surname is null && nationality is null
            && passportNo is null && dateOfBirth is null && cassFileNo is null)
            return BadRequest("At least one search parameter is required.");

        var results = await service.SearchAsync(
            new CassSearchRequest(arc, name, surname, nationality, passportNo, dateOfBirth, cassFileNo), ct);
        return Ok(results);
    }

    /// <summary>Create a new CASS record</summary>
    [HttpPost]
    [ProducesResponseType(typeof(CassSearchResponse), 201)]
    public async Task<IActionResult> Create([FromBody] CassCreateRequest req, CancellationToken ct = default)
    {
        var result = await service.CreateAsync(req, ct);
        return CreatedAtAction(nameof(Search), new { arc = result.Arc }, result);
    }
}
