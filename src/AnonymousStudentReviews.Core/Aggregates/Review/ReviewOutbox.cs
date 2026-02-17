namespace AnonymousStudentReviews.Core.Aggregates.Review;

public class ReviewOutbox
{
    public Guid Id { get; init; }
    public Guid StateId { get; set; }
    public Guid ActionId { get; init; }
    public required ReviewOutboxPayload Payload { get; init; }

    public ReviewOutboxStateEntity? State { get; set; }
    public ReviewOutboxActionEntity? Action { get; init; }
}
