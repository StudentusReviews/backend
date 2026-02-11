using FluentValidation;

namespace AnonymousStudentReviews.Api.Features.Reviews.Edit;

public class EditReviewRequestValidator : AbstractValidator<EditReviewRequest>
{
    public EditReviewRequestValidator()
    {
        RuleFor(x => x.Score).InclusiveBetween(1, 10);
        RuleFor(x => x.Body).NotEmpty().MinimumLength(20).MaximumLength(2000);
    }
}
