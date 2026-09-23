using FluentValidation;
using RECAMAS.Application.Dtos.Interoperability.Shared;

namespace RECAMAS.Application.Validators.Interoperability;

public sealed class ExternalSearchRequestValidator : AbstractValidator<ExternalSearchRequest>
{
    public ExternalSearchRequestValidator()
    {
        RuleFor(x => x).Must(x =>
                !string.IsNullOrWhiteSpace(x.Arc)
                || !string.IsNullOrWhiteSpace(x.PassportNumber)
                || (!string.IsNullOrWhiteSpace(x.FirstName)
                    && !string.IsNullOrWhiteSpace(x.LastName)
                    && (x.DateOfBirth.HasValue || (x.DateOfBirthStart.HasValue && x.DateOfBirthEnd.HasValue))))
            .WithMessage("Provide an ARC, passport number, or first name, last name and date of birth.");

        RuleFor(x => x.FirstName).MaximumLength(150);
        RuleFor(x => x.LastName).MaximumLength(150);
        RuleFor(x => x.PassportNumber).MaximumLength(50);
        RuleFor(x => x.DateOfBirth).LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow)).When(x => x.DateOfBirth.HasValue);
        RuleFor(x => x).Must(x => !x.DateOfBirth.HasValue || (!x.DateOfBirthStart.HasValue && !x.DateOfBirthEnd.HasValue))
            .WithMessage("Provide either an exact date of birth or a date range, not both.");
        RuleFor(x => x).Must(x => x.DateOfBirthStart.HasValue == x.DateOfBirthEnd.HasValue)
            .WithMessage("Both dateOfBirthStart and dateOfBirthEnd are required for a date range.");
        RuleFor(x => x).Must(x => !x.DateOfBirthStart.HasValue || x.DateOfBirthStart <= x.DateOfBirthEnd)
            .WithMessage("dateOfBirthStart must not be later than dateOfBirthEnd.");
        RuleFor(x => x.TravelDocumentType).Must(x => x is null or "P" or "I")
            .WithMessage("travelDocumentType must be P or I.");
        RuleFor(x => x.Gender).Must(x => x is null or "M" or "F" or "U")
            .WithMessage("gender must be M, F or U.");
    }
}
