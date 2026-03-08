using System.Security.Cryptography;
using System.Text;

namespace AnonymousStudentReviews.Infrastructure;

public static class HmacSha256Hasher
{
    public static string Hash(string text, string key)
    {
        var keyBytes = Convert.FromBase64String(key);

        using var hmac = new HMACSHA256(keyBytes);

        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(text));
        var hashString = Convert.ToBase64String(hash);

        return hashString;
    }
}
