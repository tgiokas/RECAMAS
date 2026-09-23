using StoplistApi.Application.DTOs;
using StoplistApi.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Domain.Enums;

namespace StoplistApi.WebApi.Controllers;

[ApiController]
[Route("api/stoplist")]
[Produces("application/json")]
public class StoplistController(StoplistService service) : ControllerBase
{
    /// <summary>
    /// Search the Stoplist / entry-ban register for a TCN (Table 167).
    /// Returns IsOnStoplist flag, UniqueEntryBanNumber and StoplistEntryDate (Table 168).
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(IEnumerable<StoplistSearchResponse>), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Search(
        [FromQuery] string? arc,
        [FromQuery] string? name,
        [FromQuery] string? surname,
        [FromQuery] Nationality? nationality,
        [FromQuery] string? passportNo,
        [FromQuery] DateTime? dateOfBirth,
        CancellationToken ct = default)
    {
        if (arc is null && name is null && surname is null &&
            nationality is null && passportNo is null && dateOfBirth is null)
            return BadRequest("At least one search parameter is required.");

        var results = await service.SearchAsync(
            new StoplistSearchRequest(arc, name, surname, nationality, passportNo, dateOfBirth), ct);
        return Ok(results);
    }

    /// <summary>
    /// Register or update a Stoplist / entry-ban entry (Table 167).
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(StoplistSearchResponse), 201)]
    public async Task<IActionResult> Create(
        [FromBody] StoplistCreateRequest req, CancellationToken ct = default)
    {
        var result = await service.CreateAsync(req, ct);
        return CreatedAtAction(nameof(Search), new { arc = result.Arc }, result);
    }
}
