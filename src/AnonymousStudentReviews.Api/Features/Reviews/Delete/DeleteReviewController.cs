using AnonymousStudentReviews.Api.Extensions;
using AnonymousStudentReviews.UseCases.Reviews.Delete;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using OpenIddict.Validation.AspNetCore;

namespace AnonymousStudentReviews.Api.Features.Reviews.Delete;

[Authorize(
    AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme,
    Roles = "Student")]
[ApiController]
[Route("api/reviews")]
public class DeleteReviewController : ControllerBase
{
    private readonly IDeleteReviewService _service;

    public DeleteReviewController(IDeleteReviewService service)
    {
        _service = service;
    }

    [HttpDelete("{reviewId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete([FromRoute] Guid reviewId)
    {
        var dto = new DeleteReviewDto { ReviewId = reviewId };

        var result = await _service.ExecuteAsync(dto);

        if (result.IsFailure)
        {
            return result.Error.ToProblemDetails(Request.Path);
        }

        return NoContent();
    }
}
