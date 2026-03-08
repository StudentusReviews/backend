using AnonymousStudentReviews.Api.Features.Reviews.Common;

using FluentValidation;

namespace AnonymousStudentReviews.Api.Features.Reviews.Create;

public class CreateReviewRequestValidator : AbstractValidator<CreateReviewRequest>
{
    public CreateReviewRequestValidator()
    {
        RuleFor(x => x.UniversityId).NotEmpty();
        RuleFor(x => x.Score).InclusiveBetween(1, 10);
        RuleFor(x => x.Body).SetValidator(new ReviewBodyValidator());
    }
}
