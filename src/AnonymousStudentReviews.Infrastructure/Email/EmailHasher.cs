using AnonymousStudentReviews.Infrastructure.Options;
using AnonymousStudentReviews.UseCases.Registration.Abstractions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace AnonymousStudentReviews.Infrastructure.Email;

public class EmailHasher : IEmailHasher
{
    private readonly IConfiguration _configuration;
    private readonly EmailSecretOptions _emailSecretOptions;

    public EmailHasher(IConfiguration configuration, IOptions<EmailSecretOptions> emailSecretOptions)
    {
        _configuration = configuration;
        _emailSecretOptions = emailSecretOptions.Value;
    }

    public string Hash(string email)
    {
        var key = _emailSecretOptions.EmailHashKey;

        return HmacSha256Hasher.Hash(email, key);
    }
}
