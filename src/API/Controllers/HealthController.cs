using Microsoft.AspNetCore.Mvc;

namespace RECAMAS.Api.Controllers;

/// <summary>
/// Thin placeholder — the real /health endpoint is wired via
/// app.MapHealthChecks("/health") in Program.cs (checks Postgres, and later
/// Authentication/Storage/Kafka connectivity). This controller exists only
/// as the first proof-of-life API endpoint for Sprint 1's "does it boot" goal.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet("ping")]
    public IActionResult Ping() => Ok(new { status = "RECAMAS.Api is running", timestampUtc = DateTimeOffset.UtcNow });
}
