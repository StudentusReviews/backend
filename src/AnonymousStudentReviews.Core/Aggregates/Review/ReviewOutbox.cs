namespace AnonymousStudentReviews.Core.Aggregates.Review;

public class ReviewOutbox
{
    public Guid Id { get; init; }
    public Guid ReviewId { get; init; }
    public Guid StateId { get; set; }

    public ReviewOutboxState? State { get; set; }
    public Review? Review { get; init; }
}
