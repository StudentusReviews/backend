using FluentValidation;

namespace AnonymousStudentReviews.Api.Features.Reviews.RetrieveMany;

public class
    RetrieveManyReviewsQueryParametersValidator : AbstractValidator<RetrieveManyReviewsQueryParameters>
{
    public RetrieveManyReviewsQueryParametersValidator()
    {
        RuleFor(x => x.Limit)
            .InclusiveBetween(1, 100);
    }
}
