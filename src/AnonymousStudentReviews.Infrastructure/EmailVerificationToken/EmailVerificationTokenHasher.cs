using AnonymousStudentReviews.UseCases.Users.Create;

using Microsoft.Extensions.Configuration;

namespace AnonymousStudentReviews.Infrastructure.EmailVerificationToken;

public class EmailVerificationTokenHasher : IEmailVerificationTokenHasher
{
    private readonly IConfiguration _configuration;

    public EmailVerificationTokenHasher(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string Hash(string token)
    {
        var key = _configuration["EmailVerificationTokenHashKey"];

        if (key is null)
        {
            throw new NullReferenceException(
                "EmailVerificationTokenHashKey is null. EmailVerificationTokenHashKey must be set in secrets.json or any other place where secrets reside");
        }

        return HmacSha256Hasher.Hash(token, key);
    }
}
