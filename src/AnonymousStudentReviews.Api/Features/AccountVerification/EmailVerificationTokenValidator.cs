using AnonymousStudentReviews.Api.Features.Dummies.Create;

using FluentValidation;

namespace AnonymousStudentReviews.Api.Features.AccountVerification;

public class EmailVerificationTokenValidator : AbstractValidator<AccountVerificationQueryParameters>
{
    public EmailVerificationTokenValidator()
    {
        RuleFor(x => x.EmailVerificationToken)
            .NotNull()
            .NotEmpty()
            .MaximumLength(256);
    }
}
