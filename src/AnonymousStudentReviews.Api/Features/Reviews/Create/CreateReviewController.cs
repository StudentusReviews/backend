using AnonymousStudentReviews.Api.Extensions;
using AnonymousStudentReviews.Api.Features.Reviews.Common;
using AnonymousStudentReviews.Core.Aggregates.Review;
using AnonymousStudentReviews.UseCases.Reviews.Create;

using FluentValidation;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using OpenIddict.Validation.AspNetCore;

namespace AnonymousStudentReviews.Api.Features.Reviews.Create;

[Authorize(
    AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme,
    Roles = "Student")]
[ApiController]
[Route("api/reviews")]
public class CreateReviewController : ControllerBase
{
    private readonly IValidator<CreateReviewRequest> _validator;
    private readonly ICreateReviewService _service;

    public CreateReviewController(IValidator<CreateReviewRequest> validator, ICreateReviewService service)
    {
        _validator = validator;
        _service = service;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ReviewResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ReviewResponse>> Create([FromBody] CreateReviewRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return validationResult.ToProblemDetails(Request.Path);
        }
        
        var dto = new CreateReviewDto
        {
            UniversityId = request.UniversityId,
            Score = request.Score,
            Body = request.Body
        };

        var result = await _service.ExecuteAsync(dto);

        if (result.IsFailure)
        {
            return result.Error.ToProblemDetails(Request.Path);
        }
        
        return CreatedAtAction(nameof(Create), ToResponse(result.Value));
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
