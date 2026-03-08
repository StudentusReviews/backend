using System.Data;

using AnonymousStudentReviews.Api.Extensions;
using AnonymousStudentReviews.Core.Aggregates.Dummy;
using AnonymousStudentReviews.UseCases.Dummies.Create;

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

    public CreateDummyController(IValidator<CreateDummyRequest> createDummyRequestValidator,
        ICreateDummyService createDummyService)
    {
        _createDummyRequestValidator = createDummyRequestValidator;
        _createDummyService = createDummyService;
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


        if (createDummyResult.IsFailure)
        {
            return createDummyResult.Error.ToProblemDetails(Request.Path);
        }

        return CreatedAtAction(nameof(CreateDummy), ResultToResponse(createDummyResult.Value));
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

    private CreateDummyResponse ResultToResponse(Dummy result)
    {
        return new CreateDummyResponse { Id = result.Id, Name = result.Name, UserId = result.UserId.ToString() };
    }
}
