using Microsoft.AspNetCore.Mvc;

namespace AnonymousStudentReviews.Api.Features.AccountVerification;

public class AccountVerificationQueryParameters
{
    [FromQuery(Name = "email-verification-token")]
    public string EmailVerificationToken { get; set; }
}
