using FluentValidation;
using RECAMAS.Application.Dtos.TCNProfile;

namespace RECAMAS.Application.Validators.TCNProfile;

public class CreateTCNProfileRequestValidator : AbstractValidator<CreateTCNProfileRequest>
{
    public CreateTCNProfileRequestValidator()
    {
        RuleFor(x => x.FirstNameEn).NotEmpty().MaximumLength(200);
        RuleFor(x => x.LastNameEn).NotEmpty().MaximumLength(200);

        RuleFor(x => x.DateOfBirth)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
            .When(x => x.DateOfBirth.HasValue)
            .WithMessage("DateOfBirth cannot be in the future.");
    }
}
