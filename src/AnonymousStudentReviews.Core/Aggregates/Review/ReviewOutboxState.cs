namespace AnonymousStudentReviews.Core.Aggregates.Review;

public enum ReviewOutboxState
{
    Pending,
    Processed
}

public static class ReviewOutboxStateExtensions
{
    public static string GetName(this ReviewOutboxState reviewOutboxState)
    {
        return reviewOutboxState switch
        {
            ReviewOutboxState.Pending => ReviewOutboxStateNameConstants.Pending,
            ReviewOutboxState.Processed => ReviewOutboxStateNameConstants.Processed,
            _ => throw new ArgumentOutOfRangeException(nameof(reviewOutboxState), reviewOutboxState, null)
        };
    }
}
