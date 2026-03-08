using Microsoft.AspNetCore.Mvc;

namespace AnonymousStudentReviews.Api.Features.AccountVerification;

public class AccountVerificationQueryParameters
{
    [FromQuery(Name = "email-verification-token")]
    public required string EmailVerificationToken { get; init; }

    [FromQuery(Name = "return-url")] public required string ReturnUrl { get; init; }
}
