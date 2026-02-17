using AnonymousStudentReviews.Core.Aggregates.Review;

namespace AnonymousStudentReviews.UseCases.Reviews.Outbox.CreateMessage;

public class CreateMessageInReviewOutboxDto
{
    public int Score { get; set; }
    public int OldScore { get; set; } = 0;
    public ReviewOutboxAction ReviewOutboxAction { get; set; }
}
