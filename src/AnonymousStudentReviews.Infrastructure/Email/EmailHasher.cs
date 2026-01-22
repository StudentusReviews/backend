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

        var keyBytes = Convert.FromBase64String(key);

        using var hmac = new HMACSHA256(keyBytes);

        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(email));
        var hashString = Convert.ToBase64String(hash);

        return hashString;
    }
}
