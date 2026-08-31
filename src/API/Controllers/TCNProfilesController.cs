using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using RECAMAS.Application.Dtos.TCNProfile;
using RECAMAS.Application.Interfaces;

namespace RECAMAS.Api.Controllers;

/// TODO: add [Authorize] once the role/policy model is decided (see Program.cs's
/// own RBAC note) — left open for now so this reference flow is testable without
/// a Keycloak token in hand.
[ApiController]
[Route("api/[controller]")]
public class TCNProfilesController : ControllerBase
{
    private readonly ITCNProfileService _tcnProfileService;
    private readonly IValidator<CreateTCNProfileRequest> _createValidator;

    public TCNProfilesController(ITCNProfileService tcnProfileService, IValidator<CreateTCNProfileRequest> createValidator)
    {
        _tcnProfileService = tcnProfileService;
        _createValidator = createValidator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTCNProfileRequest request, CancellationToken ct)
    {
        var validation = await _createValidator.ValidateAsync(request, ct);
        if (!validation.IsValid)
        {
            var errors = validation.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

            return ValidationProblem(new ValidationProblemDetails(errors));
        }

        var result = await _tcnProfileService.CreateAsync(request, ct);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
}
