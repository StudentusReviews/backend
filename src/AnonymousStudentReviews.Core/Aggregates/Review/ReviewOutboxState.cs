namespace AnonymousStudentReviews.Core.Aggregates.Review;

public enum ReviewOutboxState
{
    PendingAdd,
    PendingUpdate,
    PendingDelete,
    Processed
}

public static class ReviewOutboxStateExtensions
{
    public static string GetName(this ReviewOutboxState reviewOutboxState)
    {
        return reviewOutboxState switch
        {
            ReviewOutboxState.PendingAdd => ReviewOutboxStateNameConstants.PendingAdd,
            ReviewOutboxState.PendingUpdate => ReviewOutboxStateNameConstants.PendingUpdate,
            ReviewOutboxState.Processed => ReviewOutboxStateNameConstants.Processed,
            ReviewOutboxState.PendingDelete => ReviewOutboxStateNameConstants.PendingDelete,
            _ => throw new ArgumentOutOfRangeException(nameof(reviewOutboxState), reviewOutboxState, null)
        };
    }
}
