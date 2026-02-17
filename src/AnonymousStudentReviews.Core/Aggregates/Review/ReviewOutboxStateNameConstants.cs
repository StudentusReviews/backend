namespace AnonymousStudentReviews.Core.Aggregates.Review;

public static class ReviewOutboxStateNameConstants
{
    public const string PendingAdd = "pending_add";
    public const string PendingUpdate = "pending_update";
    public const string Processed = "processed";
}
