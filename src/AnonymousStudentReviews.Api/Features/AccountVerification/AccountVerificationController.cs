using AnonymousStudentReviews.Api.Extensions;
using AnonymousStudentReviews.Api.Features.Dummies.Create;
using AnonymousStudentReviews.UseCases.AccountVerification;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;

namespace AnonymousStudentReviews.Api.Features.AccountVerification;

[Route("verify_account")]
public class AccountVerificationController : Controller
{
    private readonly IValidator<AccountVerificationQueryParameters> _accountVerificationQueryParamsValidator;
    private readonly IAccountVerificationService _accountVerificationService;

    public AccountVerificationController(IAccountVerificationService accountVerificationService,
        IValidator<AccountVerificationQueryParameters> accountVerificationQueryParamsValidator)
    {
        _accountVerificationService = accountVerificationService;
        _accountVerificationQueryParamsValidator = accountVerificationQueryParamsValidator;
    }

    public async Task<IActionResult> ConfirmAccount(
        [FromQuery] AccountVerificationQueryParameters queryParameters)
    {
        var validationResult = await _accountVerificationQueryParamsValidator.ValidateAsync(queryParameters);

        if (!validationResult.IsValid)
        {
            return validationResult.ToProblemDetails(Request.Path);
        }

        var result = await _accountVerificationService.HandleAsync(queryParameters.EmailVerificationToken);

        if (result.IsFailure)
        {
            var errorCode = result.Error.Code;

            if (errorCode == AccountVerificationErrors.TokenNotFound.Code)
            {
                return View("Failure");
            }

            if (errorCode == AccountVerificationErrors.TokenAlreadyUsed.Code)
            {
                return View("Failure");
            }

            if (errorCode == AccountVerificationErrors.TokenExpired.Code)
            {
                return View("Failure");
            }
        }

        return View("Success");
    }
}
