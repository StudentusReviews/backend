using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.Review;
using AnonymousStudentReviews.UseCases.Utils;

namespace AnonymousStudentReviews.UseCases.Reviews.Retrieve;

public class RetrieveManyReviewsService : IRetrieveManyReviewsService
{
    private readonly IReviewRepository _reviewRepository;

    public RetrieveManyReviewsService(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<CursorPagedResult<Review>> HandleAsync(RetrieveManyReviewsDto dto)
    {
        var cursor = CursorUtils.FromCursor<ReviewCursor>(dto.Cursor);

        var result = await _reviewRepository.GetAllAsync(
            dto.UniversityId, dto.SortOrder, cursor, dto.Limit);

        return result;
    }
}
