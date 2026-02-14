using FluentValidation;

namespace AnonymousStudentReviews.Api.Features.ApplicationToAddAUniversity.Create;

public class CreateApplicationToAddAUniversityRequestValidator : AbstractValidator<CreateApplicationToAddAUniversityRequest>
{
    public CreateApplicationToAddAUniversityRequestValidator()
    {
        RuleFor(x => x.UniversityName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.DomainName)
            .NotEmpty()
            .MaximumLength(50)
            /*.Must(name => name.Contains("@"))
            .WithMessage("DomainName має містити символ '@'")*/;
    }
}
