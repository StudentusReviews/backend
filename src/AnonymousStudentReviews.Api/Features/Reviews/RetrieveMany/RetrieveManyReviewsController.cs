using AnonymousStudentReviews.Api.Extensions;
using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.Review;
using AnonymousStudentReviews.UseCases.Reviews.RetrieveMany;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;

namespace AnonymousStudentReviews.Api.Features.Reviews.RetrieveMany;

[Route("api/reviews")]
[ApiController]
public class RetrieveManyReviewsController : ControllerBase
{
    private readonly IRetrieveManyReviewsService _manyReviewsService;
    private readonly IValidator<RetrieveManyReviewsQueryParameters> _validator;

    public RetrieveManyReviewsController(IValidator<RetrieveManyReviewsQueryParameters> validator,
        IRetrieveManyReviewsService manyReviewsService)
    {
        _validator = validator;
        _manyReviewsService = manyReviewsService;
    }

    [HttpGet]
    public async Task<ActionResult<CursorPagedResult<Review>>> GetManyReviews(
        [FromQuery] RetrieveManyReviewsQueryParameters queryParameters)
    {
        var validationResult = await _validator.ValidateAsync(queryParameters);

        if (!validationResult.IsValid)
        {
            return validationResult.ToProblemDetails(Request.Path);
        }

        var serviceResult = await _manyReviewsService.HandleAsync(RequestToDto(queryParameters));

        return Ok(serviceResult);
    }

    private RetrieveManyReviewsDto RequestToDto(RetrieveManyReviewsQueryParameters request)
    {
        return new RetrieveManyReviewsDto
        {
            Limit = request.Limit,
            Cursor = request.Cursor,
            UniversityId = request.UniversityId,
            SortOrder = request.SortOrder
        };
    }
}
