using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.AllowedEmailDomain;
using AnonymousStudentReviews.UseCases.Registration.Abstractions;

using Microsoft.Extensions.Logging;

namespace AnonymousStudentReviews.UseCases.Registration;

public class RegistrationService : IRegistrationService
{
    private readonly ILogger<RegistrationService> _logger;
    private readonly IUserManager _userManager;

    public RegistrationService(IAllowedEmailDomainRepository allowedEmailDomainRepository,
        ILogger<RegistrationService> logger,
        IUserManager userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<Result> HandleAsync(RegistrationDto dto)
    {
        _logger.LogInformation("Create user service started");

        var createUserResult = await _userManager.CreateAsync(dto.Email, dto.Password);

        _logger.LogInformation("Requested account verification");

        if (createUserResult.IsSuccess)
        {
            var createdUser = createUserResult.Value;
            await _userManager.RequestAccountVerificationAsync(createdUser, dto.Email, dto.ReturnUrl);
        }

        return Result.Success();
    }
}
