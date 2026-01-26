using System.Security.Cryptography;
using System.Text;

using AnonymousStudentReviews.UseCases.Users.Create;

using Microsoft.Extensions.Configuration;

namespace AnonymousStudentReviews.Infrastructure.Email;

public class EmailHasher : IEmailHasher
{
    private readonly IConfiguration _configuration;

    public EmailHasher(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string Hash(string email)
    {
        var key = _configuration["EmailHashKey"];

        if (key is null)
        {
            throw new NullReferenceException(
                "EmailHashKey is null. EmailHashKey must be set in secrets.json or any other place where secrets reside");
        }

        return HmacSha256Hasher.Hash(email, key);
    }
}
