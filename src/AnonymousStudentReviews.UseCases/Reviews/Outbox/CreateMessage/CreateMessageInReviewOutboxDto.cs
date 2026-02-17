using AnonymousStudentReviews.Core.Aggregates.Review;

namespace AnonymousStudentReviews.UseCases.Reviews.Outbox.CreateMessage;

public class CreateMessageInReviewOutboxDto
{
    public required Review Review { get; init; }
    public ReviewOutboxState ReviewOutboxState { get; set; }
}
