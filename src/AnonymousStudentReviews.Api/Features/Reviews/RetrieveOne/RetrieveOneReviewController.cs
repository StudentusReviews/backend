using AnonymousStudentReviews.Api.Extensions;
using AnonymousStudentReviews.Core.Aggregates.Review;
using AnonymousStudentReviews.UseCases.Reviews.RetrieveOne;

using Microsoft.AspNetCore.Mvc;

namespace AnonymousStudentReviews.Api.Features.Reviews.RetrieveOne;

[Route("api/reviews/{reviewId:guid}")]
[ApiController]
public class RetrieveOneReviewController : ControllerBase
{
    private readonly IRetrieveOneReviewService _retrieveOneReviewService;

    public RetrieveOneReviewController(IRetrieveOneReviewService retrieveOneReviewService)
    {
        _retrieveOneReviewService = retrieveOneReviewService;
    }

    [HttpGet]
    public async Task<ActionResult<ReviewPreview>> GetReviewAsync([FromRoute] Guid reviewId)
    {
        var result = await _retrieveOneReviewService.HandleAsync(reviewId);

        if (result.IsFailure)
        {
            return result.Error.ToProblemDetails(Request.Path);
        }

        return Ok(result.Value);
    }
}
