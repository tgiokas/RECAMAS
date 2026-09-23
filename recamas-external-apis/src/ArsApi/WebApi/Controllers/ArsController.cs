using ArsApi.Application.DTOs;
using ArsApi.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Domain.Enums;

namespace ArsApi.WebApi.Controllers;

/// <summary>
/// ARS Integration API — Alien Registration System
/// Implements §9.2 of the CIT-RECAMAS Implementation Study
/// </summary>
[ApiController]
[Route("api/ars")]
[Produces("application/json")]
public class ArsController(ArsService service) : ControllerBase
{
    /// <summary>
    /// Search ARS records by one or more criteria (Table 151).
    /// At least one parameter must be provided.
    /// Supports partial matching on text fields.
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(IEnumerable<ArsSearchResponse>), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Search(
        [FromQuery] string? arc,
        [FromQuery] string? name,
        [FromQuery] string? surname,
        [FromQuery] Nationality? nationality,
        [FromQuery] string? passportNo,
        [FromQuery] DateTime? dateOfBirth,
        [FromQuery] string? mdFileNumber,
        CancellationToken ct = default)
    {
        if (arc is null && name is null && surname is null &&
            nationality is null && passportNo is null &&
            dateOfBirth is null && mdFileNumber is null)
            return BadRequest("At least one search parameter is required.");

        var req = new ArsSearchRequest(arc, name, surname, nationality, passportNo, dateOfBirth, mdFileNumber);
        var results = await service.SearchAsync(req, ct);
        return Ok(results);
    }

    /// <summary>
    /// Create a new ARS record (Tables 151-155).
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ArsSearchResponse), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] ArsCreateRequest req, CancellationToken ct = default)
    {
        var result = await service.CreateAsync(req, ct);
        return CreatedAtAction(nameof(Search), new { arc = result.Arc }, result);
    }

    /// <summary>
    /// Returns sets of ARS records that share an identifying attribute
    /// (same surname, same date of birth, same surname + country of origin,
    /// or same travel document issuing country) and may therefore represent
    /// the same or a related TCN.
    /// </summary>
    [HttpGet("potential-duplicates")]
    [ProducesResponseType(typeof(IEnumerable<ArsDuplicateGroup>), 200)]
    public async Task<IActionResult> GetPotentialDuplicates(CancellationToken ct = default)
    {
        var results = await service.GetPotentialDuplicatesAsync(ct);
        return Ok(results);
    }

    /// <summary>
    /// Returns groups of ARS records that share the same ARS folder number,
    /// simulating potential family/case-file relationships between TCNs
    /// (RECAMAS §2.2.4 "Linked Profile Suggestion").
    /// </summary>
    [HttpGet("folder-groups")]
    [ProducesResponseType(typeof(IEnumerable<ArsFolderGroup>), 200)]
    public async Task<IActionResult> GetFolderGroups(CancellationToken ct = default)
    {
        var results = await service.GetFolderGroupsAsync(ct);
        return Ok(results);
    }
}
