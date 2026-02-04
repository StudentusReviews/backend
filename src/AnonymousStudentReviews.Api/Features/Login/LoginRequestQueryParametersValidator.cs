using AnonymousStudentReviews.Api.Extensions;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;

namespace AnonymousStudentReviews.Api.Features.Login;

public class LoginRequestQueryParametersValidator : AbstractValidator<LoginRequestQueryParameters>
{
    public LoginRequestQueryParametersValidator()
    {
        RuleFor(x => x.ReturnUrl)
            .NotNull()
            .NotEmpty()
            .IsRelativeUri()
            .Must(returnUrl => returnUrl.StartsWith("/connect/authorize", StringComparison.Ordinal));
    }
}
