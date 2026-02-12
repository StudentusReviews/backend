using System.Data;

using AnonymousStudentReviews.Api.Extensions;
using AnonymousStudentReviews.UseCases.AppToAddAUni.Create;

using FluentValidation;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using OpenIddict.Validation.AspNetCore;

namespace AnonymousStudentReviews.Api.Features.AppToAddAUni.Create;

[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/applications")]
public class CreateAppToAddAUniController : ControllerBase
{
    private readonly IValidator<CreateAppToAddAUniRequest> _createAppToAddAUniRequestValidator;
    private readonly ICreateAppToAddAUniService _createAppToAddAUniService;

    public CreateAppToAddAUniController(IValidator<CreateAppToAddAUniRequest> createAppToAddAUniRequestValidator,
        ICreateAppToAddAUniService createAppToAddAUniService)
    {
        _createAppToAddAUniRequestValidator = createAppToAddAUniRequestValidator;
        _createAppToAddAUniService = createAppToAddAUniService;
    }

    [HttpPost]
    public async Task<ActionResult<CreateAppToAddAUniResponse>> CreateAppToAddAUni([FromBody] CreateAppToAddAUniRequest request)
    {
        var validationResult = await _createAppToAddAUniRequestValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return validationResult.ToProblemDetails(Request.Path);
        }

        var createAppToAddAUniResult = await _createAppToAddAUniService.ExecuteAsync(RequestToDto(request));


        if (createAppToAddAUniResult.IsFailure)
        {
            return createAppToAddAUniResult.Error.ToProblemDetails(Request.Path);
        }

        return CreatedAtAction(nameof(CreateAppToAddAUni), ResultToResponse(createAppToAddAUniResult.Value));
    }

    private CreateAppToAddAUniDto RequestToDto(CreateAppToAddAUniRequest request)
    {
        return new CreateAppToAddAUniDto { UniversityName = request.UniversityName, DomainName = request.DomainName };
    }

    private CreateAppToAddAUniResponse ResultToResponse(Core.Aggregates.AppToAddAUni.AppToAddAUni result)
    {
        return new CreateAppToAddAUniResponse { Id = result.Id, UniversityName = result.UniversityName, DomainName = result.DomainName, CreatedAt = result.CreatedAt,  UserId = result.UserId };
    }
}
