using System.Data;

using AnonymousStudentReviews.Api.Extensions;
using AnonymousStudentReviews.UseCases.ApplicationToAddAUniversity.Create;

using FluentValidation;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using OpenIddict.Validation.AspNetCore;

namespace AnonymousStudentReviews.Api.Features.ApplicationToAddAUniversity.Create;

[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/applications/create")]
public class CreateApplicationToAddAUniversityController : ControllerBase
{
    private readonly IValidator<CreateApplicationToAddAUniversityRequest> _createApplicationToAddAUniversityRequestValidator;
    private readonly ICreateApplicationToAddAUniversityService _createApplicationToAddAUniversityService;

    public CreateApplicationToAddAUniversityController(IValidator<CreateApplicationToAddAUniversityRequest> createAppToAddAUniRequestValidator,
        ICreateApplicationToAddAUniversityService createApplicationToAddAUniversityService)
    {
        _createApplicationToAddAUniversityRequestValidator = createAppToAddAUniRequestValidator;
        _createApplicationToAddAUniversityService = createApplicationToAddAUniversityService;
    }

    [HttpPost]
    public async Task<ActionResult<CreateApplicationToAddAUniversityResponse>> CreateAppToAddAUni([FromBody] CreateApplicationToAddAUniversityRequest request)
    {
        var validationResult = await _createApplicationToAddAUniversityRequestValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return validationResult.ToProblemDetails(Request.Path);
        }

        var createApplicationToAddAUniversityResult = await _createApplicationToAddAUniversityService.ExecuteAsync(RequestToDto(request));


        if (createApplicationToAddAUniversityResult.IsFailure)
        {
            return createApplicationToAddAUniversityResult.Error.ToProblemDetails(Request.Path);
        }

        return CreatedAtAction(nameof(CreateAppToAddAUni), ResultToResponse(createApplicationToAddAUniversityResult.Value));
    }

    private CreateApplicationToAddAUniversityDto RequestToDto(CreateApplicationToAddAUniversityRequest request)
    {
        return new CreateApplicationToAddAUniversityDto { UniversityName = request.UniversityName, DomainName = request.DomainName };
    }

    private CreateApplicationToAddAUniversityResponse ResultToResponse(Core.Aggregates.ApplicationToAddAUniversity.Base.ApplicationToAddAUniversity result)
    {
        return new CreateApplicationToAddAUniversityResponse { Id = result.Id, UniversityName = result.UniversityName, DomainName = result.DomainName, CreatedAt = result.CreatedAt,  UserId = result.UserId };
    }
}
