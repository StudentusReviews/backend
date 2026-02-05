using AnonymousStudentReviews.UseCases.Registration.Abstractions;

namespace AnonymousStudentReviews.Api.Features.AccountVerification;

public class AccountVerificationLinkFactory : IAccountVerificationLinkFactory
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly LinkGenerator _linkGenerator;

    public AccountVerificationLinkFactory(IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator)
    {
        _httpContextAccessor = httpContextAccessor;
        _linkGenerator = linkGenerator;
    }

    public string Create(string emailVerificationToken)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext is null)
        {
            throw new InvalidOperationException("HTTP context accessor is null. Should never happen");
        }

        var accountVerificationLink = _linkGenerator.GetUriByAction(
            httpContext,
            controller: "AccountVerification",
            action: "ConfirmAccount",
            values: new RouteValueDictionary()
            {
                ["email-verification-token"] = emailVerificationToken
            });

        if (accountVerificationLink is null)
        {
            throw new InvalidOperationException("Account confirmation link can't be created. Should never happen");
        }

        return accountVerificationLink;
    }
}
