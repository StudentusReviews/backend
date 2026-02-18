using FluentValidation;

namespace AnonymousStudentReviews.Api.Features.Universities.RetrieveMany;

public class
    RetrieveManyUniversitiesQueryParametersValidator : AbstractValidator<RetrieveManyUniversitiesQueryParameters>
{
    public RetrieveManyUniversitiesQueryParametersValidator()
    {
        RuleFor(x => x.Limit)
            .InclusiveBetween(1, 100);

        RuleFor(x => x.City)
            .MinimumLength(1)
            .MaximumLength(255);

        RuleFor(x => x.Name)
            .MinimumLength(1)
            .MaximumLength(255);
    }
}
