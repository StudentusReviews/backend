using AnonymousStudentReviews.Infrastructure.Options;
using AnonymousStudentReviews.UseCases.Users.Create.Abstractions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace AnonymousStudentReviews.Infrastructure.EmailVerificationToken;

public class EmailVerificationTokenHasher : IEmailVerificationTokenHasher
{
    private readonly IConfiguration _configuration;
    private readonly EmailSecretOptions _emailSecretOptions;

    public EmailVerificationTokenHasher(IConfiguration configuration, IOptions<EmailSecretOptions> emailSecretOptions)
    {
        _configuration = configuration;
        _emailSecretOptions = emailSecretOptions.Value;
    }

    public string Hash(string token)
    {
        var key = _emailSecretOptions.EmailVerificationTokenHashKey;

        return HmacSha256Hasher.Hash(token, key);
    }
}
