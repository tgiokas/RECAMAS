using System.Security.Claims;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using RECAMAS.Application.Dtos.Interoperability.Shared;
using RECAMAS.Application.Interfaces.Interoperability;

namespace RECAMAS.Api.Controllers;

[ApiController]
[Route("api/tcn-search")]
public sealed class TCNSearchController(
    ITcnSearchOrchestrator orchestrator,
    IValidator<ExternalSearchRequest> validator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Search([FromBody] ExternalSearchRequest request, CancellationToken cancellationToken)
    {
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            var errors = validation.Errors.GroupBy(x => x.PropertyName)
                .ToDictionary(x => x.Key, x => x.Select(y => y.ErrorMessage).ToArray());
            return ValidationProblem(new ValidationProblemDetails(errors));
        }

        var correlationId = Request.Headers["X-Correlation-ID"].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(correlationId))
            correlationId = HttpContext.TraceIdentifier;
        Response.Headers["X-Correlation-ID"] = correlationId;

        long? userId = long.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var parsedUserId)
            ? parsedUserId
            : null;
        var context = new ExternalRequestContext(correlationId, TriggeredByUserId: userId, Caller: "TCNProfile");
        var result = await orchestrator.SearchAsync(request, context, cancellationToken);

        return result.Ars.Success ? Ok(result) : StatusCode(result.Ars.StatusCode ?? StatusCodes.Status502BadGateway, result);
    }
}
