using System.Data;

using AnonymousStudentReviews.Api.Extensions;
using AnonymousStudentReviews.UseCases.Dummies.Create;
using AnonymousStudentReviews.UseCases.Registration.Abstractions;

using FluentValidation;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using OpenIddict.Validation.AspNetCore;

namespace AnonymousStudentReviews.Api.Features.Dummies.Create;

[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/dummies")]
public class CreateDummyController : ControllerBase
{
    private readonly IValidator<CreateDummyRequest> _createDummyRequestValidator;
    private readonly ICreateDummyService _createDummyService;
    private readonly ILogger<CreateDummyController> _logger;
    private readonly IUserManager _userManager;

    public CreateDummyController(IValidator<CreateDummyRequest> createDummyRequestValidator,
        ICreateDummyService createDummyService, ILogger<CreateDummyController> logger, IUserManager userManager)
    {
        _createDummyRequestValidator = createDummyRequestValidator;
        _createDummyService = createDummyService;
        _logger = logger;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<ActionResult<CreateDummyResponse>> CreateDummy([FromBody] CreateDummyRequest request)
    {
        var validationResult = await _createDummyRequestValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return validationResult.ToProblemDetails(Request.Path);
        }

        var createDummyResult = await _createDummyService.ExecuteAsync(RequestToDto(request));
        
        var user = await _userManager.GetUserAsync(User);

        if ( user.IsFailure)
        {
            _logger.LogError("user is null");
        }
        else
        {
            _logger.LogInformation($"user is not null, user id = {user.Value.Id}");
        }

        if (createDummyResult.IsFailure)
        {
            return createDummyResult.Error.ToProblemDetails(Request.Path);
        }

        return CreatedAtAction(nameof(CreateDummy), createDummyResult.Value);
    }

    [HttpGet]
    public async Task<ActionResult> TestException()
    {
        throw new DBConcurrencyException();
    }

    private CreateDummyDto RequestToDto(CreateDummyRequest request)
    {
        return new CreateDummyDto { Name = request.Name };
    }
}
