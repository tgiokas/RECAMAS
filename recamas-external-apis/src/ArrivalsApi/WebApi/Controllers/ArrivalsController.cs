using ArrivalsApi.Application.DTOs;
using ArrivalsApi.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Domain.Enums;

namespace ArrivalsApi.WebApi.Controllers;

[ApiController]
[Route("api/arrivals")]
[Produces("application/json")]
public class ArrivalsController(ArrivalsService service) : ControllerBase
{
    /// <summary>
    /// Search arrivals/departures movement history for a TCN.
    /// Returns ALL movement records matching the search criteria (Table 164/165).
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(IEnumerable<ArrivalMovementResponse>), 200)]
    public async Task<IActionResult> Search(
        [FromQuery] string? arc, [FromQuery] string? name, [FromQuery] string? surname,
        [FromQuery] Nationality? nationality, [FromQuery] string? passportNo,
        [FromQuery] DateTime? dateOfBirth, CancellationToken ct = default)
    {
        if (arc is null && name is null && surname is null && nationality is null
            && passportNo is null && dateOfBirth is null)
            return BadRequest("At least one search parameter is required.");

        var results = await service.SearchAsync(
            new ArrivalsSearchRequest(arc, name, surname, nationality, passportNo, dateOfBirth), ct);
        return Ok(results);
    }

    /// <summary>
    /// Register a new arrival or departure movement record (Table 164).
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ArrivalMovementResponse), 201)]
    public async Task<IActionResult> Create(
        [FromBody] ArrivalsCreateRequest req, CancellationToken ct = default)
    {
        var result = await service.CreateAsync(req, ct);
        return CreatedAtAction(nameof(Search), new { arc = result.Arc }, result);
    }
}
