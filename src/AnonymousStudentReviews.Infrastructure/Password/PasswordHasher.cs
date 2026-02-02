using AnonymousStudentReviews.UseCases.Registration.Abstractions;
using AnonymousStudentReviews.UseCases.Users.Create;

using Microsoft.AspNetCore.Identity;

namespace AnonymousStudentReviews.Infrastructure.Password;

public class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<PlaceholderUserClass> _identityPasswordHasher;

    public PasswordHasher()
    {
        _identityPasswordHasher = new PasswordHasher<PlaceholderUserClass>();
    }

    public string HashPassword(string password)
    {
        var hashedPassword = _identityPasswordHasher.HashPassword(new PlaceholderUserClass(), password);
        return hashedPassword;
    }

    public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
    {
        var result = _identityPasswordHasher
            .VerifyHashedPassword(new PlaceholderUserClass(), hashedPassword, providedPassword);

        return result == PasswordVerificationResult.Success;
    }
}
