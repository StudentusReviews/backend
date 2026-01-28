using Microsoft.AspNetCore.Mvc;

namespace AnonymousStudentReviews.Api.Features.Dummies.Create;

public class AccountVerificationQueryParameters
{
    [FromQuery(Name = "email-verification-token")]
    public string EmailVerificationToken { get; set; }
}
