using Microsoft.AspNetCore.Mvc;

namespace AnonymousStudentReviews.Api.Features.Login;

public class LoginRequestQueryParameters
{
    [FromQuery(Name = "return-url")] public string ReturnUrl { get; set; }
}
