namespace AnonymousStudentReviews.Core.Aggregates.Review;

public class ReviewOutboxStateEntity
{
    public Guid Id { get; init; }
    public required string Name { get; set; }

    public ICollection<ReviewOutbox> ReviewOutboxItems { get; set; } = [];
}
