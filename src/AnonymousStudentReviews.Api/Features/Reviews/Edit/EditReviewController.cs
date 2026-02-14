using AnonymousStudentReviews.Api.Extensions;
using AnonymousStudentReviews.Api.Features.Reviews.Common;
using AnonymousStudentReviews.Core.Aggregates.Review;
using AnonymousStudentReviews.UseCases.Reviews.Edit;

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
    private readonly IEditReviewService _service;

    public EditReviewController(IValidator<EditReviewRequest> validator, IEditReviewService service)
    {
        _validator = validator;
        _service = service;
    }

    [HttpPut("{reviewId:guid}")]
    [ProducesResponseType(typeof(ReviewResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReviewResponse>> Edit([FromRoute] Guid reviewId,
        [FromBody] EditReviewRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return validationResult.ToProblemDetails(Request.Path);
        }

        var dto = new EditReviewDto { ReviewId = reviewId, Score = request.Score, Body = request.Body };

        var result = await _service.ExecuteAsync(dto);

        if (result.IsFailure)
        {
            return result.Error.ToProblemDetails(Request.Path);
        }

        return Ok(ToResponse(result.Value));
    }

    private static ReviewResponse ToResponse(Review review)
        {
            return new ReviewResponse
            {
                Id = review.Id,
                UniversityId = review.UniversityId,
                UserId = review.UserId,
                Score = review.Score,
                Body = review.Body,
                CreatedAt = review.CreatedAt,
                UpdatedAt = review.UpdatedAt
            };
        }
    }
