using System.Collections.Immutable;
using System.Security.Claims;

using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.User;
using AnonymousStudentReviews.UseCases.Login;
using AnonymousStudentReviews.UseCases.Login.Abstractions;
using AnonymousStudentReviews.UseCases.Registration.Abstractions;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

using OpenIddict.Abstractions;

namespace AnonymousStudentReviews.Infrastructure.Users;

public class SignInManager : ISignInManager
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SignInManager(IPasswordHasher passwordHasher, IHttpContextAccessor httpContextAccessor)
    {
        _passwordHasher = passwordHasher;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task SignOutAsync()
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            throw new InvalidOperationException("HTTPContext is not available");
        }
        
        await _httpContextAccessor.HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public async Task<bool> CanSignInAsync(User user)
    {
        // TODO: implement the CanSignAsync method properly
        return true;
    }

    public async Task<Result> CheckPasswordSignInAsync(User user, string password)
    {
        var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user.PasswordHash, password);

        if (!passwordVerificationResult)
        {
            return Result.Failure(LoginErrors.WrongPassword);
        }

        return Result.Success();
    }

    public async Task SignInAsync(User user, bool isPersistent)
    {
        var claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

        var userRoleNames = user.Roles.Select(role => role.Name).ToImmutableArray();

        claimsIdentity.SetClaim(OpenIddictConstants.Claims.Subject, user.Id.ToString());
        claimsIdentity.SetClaims(OpenIddictConstants.Claims.Role, userRoleNames);

        var principal = new ClaimsPrincipal(claimsIdentity);

        if (_httpContextAccessor.HttpContext is null)
        {
            throw new InvalidOperationException("HTTPContext is not available");
        }
        
        await _httpContextAccessor.HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties
            {
                IsPersistent = isPersistent,
                ExpiresUtc = isPersistent
                    ? DateTimeOffset.UtcNow.AddDays(7) // TODO: use values from config
                    : DateTimeOffset.UtcNow.AddHours(1) // TODO: use values from config
            });
    }
}
