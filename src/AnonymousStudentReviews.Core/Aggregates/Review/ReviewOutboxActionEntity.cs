namespace AnonymousStudentReviews.Core.Aggregates.Review;

public class ReviewOutboxActionEntity
{
    public Guid Id { get; init; }
    public string Name { get; set; }

    public ICollection<ReviewOutbox>? ReviewOutboxes { get; set; }
}
