using AnonymousStudentReviews.Core.Aggregates.Review;
using AnonymousStudentReviews.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;

namespace AnonymousStudentReviews.Infrastructure.Reviews;

public class ReviewOutboxRepository : IReviewOutboxRepository
{
    private readonly ApplicationDbContext _context;

    public ReviewOutboxRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Create(ReviewOutbox reviewOutbox)
    {
        _context.ReviewOutbox.Add(reviewOutbox);
    }

    public async Task<ReviewOutboxStateEntity> GetReviewOutboxStateEntityAsync(ReviewOutboxState state)
    {
        var reviewOutboxStateName = state.GetName();

        var result =
            await _context.ReviewOutboxStates.FirstOrDefaultAsync(outboxState =>
                outboxState.Name == reviewOutboxStateName);

        if (result is null)
        {
            throw new Exception("Review outbox state could not be found by name. Seeding failure.");
        }

        return result;
    }

    public async Task<bool> ReviewOutboxExistsAsync(ReviewOutboxStateEntity state, Review review)
    {
        return await _context.ReviewOutbox.AnyAsync(reviewOutbox =>
            reviewOutbox.ReviewId == review.Id && reviewOutbox.StateId == state.Id);
    }
}
