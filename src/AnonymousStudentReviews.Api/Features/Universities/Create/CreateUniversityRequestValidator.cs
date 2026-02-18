using FluentValidation;

namespace AnonymousStudentReviews.Api.Features.Universities.Create;

public class CreateUniversityRequestValidator : AbstractValidator<CreateUniversityRequest>
{
    public CreateUniversityRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
        RuleFor(x => x.City).MaximumLength(255);
        RuleFor(x => x.Website).MaximumLength(300);
        RuleFor(x => x.Description).MaximumLength(1000);
    }
}
