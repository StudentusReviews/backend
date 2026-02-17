namespace AnonymousStudentReviews.Core.Aggregates.Review;

public class ReviewOutboxPayload
{
    public int Score { get; init; }
    public int OldScore { get; init; } = 0;
}
