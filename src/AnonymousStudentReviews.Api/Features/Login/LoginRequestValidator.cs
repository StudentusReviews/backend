using AnonymousStudentReviews.Api.Features.Registration;

using FluentValidation;

namespace AnonymousStudentReviews.Api.Features.Login;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .NotNull()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.RememberMe)
            .NotEmpty()
            .NotNull();
    }
}
