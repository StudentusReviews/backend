using AnonymousStudentReviews.Api.Extensions;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;

namespace AnonymousStudentReviews.Api.Features.Login;

public class LoginRequestQueryParametersValidator : AbstractValidator<LoginRequestQueryParameters>
{
    public LoginRequestQueryParametersValidator(IUrlHelper helper, IHttpContextAccessor httpContextAccessor)
    {
        RuleFor(x => x.ReturnUrl)
            .IsRelativeUri();
    }
}
