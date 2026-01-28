using System.Security.Cryptography;

using AnonymousStudentReviews.UseCases.Users.Create;
using AnonymousStudentReviews.UseCases.Users.Create.Abstractions;

namespace AnonymousStudentReviews.Infrastructure.EmailVerificationToken;

public class EmailVerificationTokenGenerator : IEmailVerificationTokenGenerator
{
    public string Generate()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        var token = Convert.ToBase64String(bytes)
            .Replace("+", "-")
            .Replace("/", "_")
            .TrimEnd('=');

        return token;
    }
}
