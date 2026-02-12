using AnonymousStudentReviews.Api.Extensions;
using AnonymousStudentReviews.Api.Features.Reviews.Common;

using FluentValidation;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using OpenIddict.Validation.AspNetCore;

namespace AnonymousStudentReviews.Api.Features.Reviews.Edit;

[Authorize(
    AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme,
    Roles = "Student")]
[ApiController]
[Route("api/reviews")]
public class EditReviewController : ControllerBase
{
    private readonly IValidator<EditReviewRequest> _validator;

    public EditReviewController(IValidator<EditReviewRequest> validator)
    {
        _validator = validator;

    }

    [HttpPut("{reviewId:guid}")]
    [ProducesResponseType(typeof(ReviewResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public async Task<ActionResult<ReviewResponse>> Edit([FromRoute] Guid reviewId, [FromBody] EditReviewRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return validationResult.ToProblemDetails(Request.Path);
        }

        return StatusCode(StatusCodes.Status501NotImplemented);

    }

}
