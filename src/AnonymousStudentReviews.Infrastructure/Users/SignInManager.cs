using System.Collections.Immutable;
using System.Security.Claims;

using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.User;
using AnonymousStudentReviews.Infrastructure.Options;
using AnonymousStudentReviews.UseCases.Login;
using AnonymousStudentReviews.UseCases.Login.Abstractions;
using AnonymousStudentReviews.UseCases.Registration.Abstractions;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

using OpenIddict.Abstractions;

namespace AnonymousStudentReviews.Infrastructure.Users;

public class SignInManager : ISignInManager
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly LoginOptions _loginOptions;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    public SignInManager(IPasswordHasher passwordHasher, IHttpContextAccessor httpContextAccessor,
        IUserRepository userRepository, IOptions<LoginOptions> loginOptions, IUnitOfWork unitOfWork)
    {
        _passwordHasher = passwordHasher;
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _loginOptions = loginOptions.Value;
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
        if (!_userRepository.IsUserEntityTracked(user))
        {
            throw new InvalidOperationException("Provided user entity must be tracked");
        }

        var isUserLockedOut = user.LockoutEnd is not null && user.LockoutEnd >= DateTimeOffset.UtcNow;


        return user is { IsBanned: false, EmailConfirmed: true } && !isUserLockedOut;
    }

    public async Task<Result> CheckPasswordSignInAsync(User user, string password)
    {
        if (!_userRepository.IsUserEntityTracked(user))
        {
            throw new InvalidOperationException("Provided user entity must be tracked");
        }

        var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user.PasswordHash, password);

        if (!passwordVerificationResult)
        {
            _userRepository.IncrementAccessFailedCount(user);

            if (user.AccessFailedCount >= _loginOptions.MaxFailedAccessAttempts)
            {
                _userRepository.LockOutUser(user);
            }

            await _unitOfWork.SaveChangesAsync();
            return Result.Failure(LoginErrors.WrongPassword);
        }

        if (user.AccessFailedCount != 0)
        {
            user.AccessFailedCount = 0;
            await _unitOfWork.SaveChangesAsync();
        }

        return Result.Success();
    }

    public async Task SignInAsync(User user, bool isPersistent)
    {
        var claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

        var userRoleNames = user.Roles.Select(role => role.Name).ToImmutableArray();

        claimsIdentity.SetClaim(OpenIddictConstants.Claims.Subject, user.Id.ToString());
        claimsIdentity.SetClaim(ClaimTypes.NameIdentifier, user.Id.ToString());

        claimsIdentity.SetClaims(OpenIddictConstants.Claims.Role, userRoleNames);
        foreach (var role in userRoleNames)
        {
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
        }

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
