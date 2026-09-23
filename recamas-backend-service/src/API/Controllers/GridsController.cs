using Microsoft.AspNetCore.Mvc;

using RECAMAS.Application.Grids.Abstractions;
using RECAMAS.Application.Grids.Models;

namespace RECAMAS.Api.Controllers;

/// <summary>
/// API endpoint that serves server-side data for configurable grids and tables.
/// </summary>
/// <remarks>
/// Routed under <c>api/grids</c>. Each grid is identified by a <c>gridKey</c> that resolves
/// to a registered <see cref="IGridSourceProviderMarker"/>.
/// </remarks>
[ApiController]
[Route("api/grids")]
public class GridsController : ControllerBase
{
    private readonly IGridQueryService _gridQueryService;

    public GridsController(IGridQueryService gridQueryService)
    {
        _gridQueryService = gridQueryService;
    }

    /// <summary>
    /// Executes a server-side query for the grid identified by <paramref name="gridKey"/> and
    /// returns the matching rows.
    /// </summary>
    [HttpPost("{gridKey}")]
    public async Task<IActionResult> GetGrid(string gridKey, [FromBody] DataGridRequest request, CancellationToken ct)
    {
        var provider = _gridQueryService.GetProvider(gridKey);
        if (provider is null)
            return NotFound();

        var result = await _gridQueryService.QueryAsync(gridKey, request, ct);
        return Ok(result);
    }
}
