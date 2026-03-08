using FluentValidation;

namespace AnonymousStudentReviews.Api.Features.Reviews.Common;

public class ReviewBodyValidator : AbstractValidator<string>
{
    public ReviewBodyValidator()
    {
        RuleFor(x => x)
            .NotNull().WithMessage("Body can't be null.")
            .NotEmpty().WithMessage("Body is required.")
            .MinimumLength(1).WithMessage("Body must be at least 1 character long.")
            .MaximumLength(4000).WithMessage("Body must not exceed 4000 characters.");
    }
}
