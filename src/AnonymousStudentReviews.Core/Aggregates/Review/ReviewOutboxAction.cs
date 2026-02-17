namespace AnonymousStudentReviews.Core.Aggregates.Review;

public enum ReviewOutboxAction
{
    Add,
    Update,
    Delete
}

public static class ReviewOutboxActionExtensions
{
    public static string GetName(this ReviewOutboxAction action)
    {
        return action switch
        {
            ReviewOutboxAction.Add => ReviewOutboxActionNameConstants.Add,
            ReviewOutboxAction.Update => ReviewOutboxActionNameConstants.Update,
            ReviewOutboxAction.Delete => ReviewOutboxActionNameConstants.Delete,
            _ => throw new ArgumentOutOfRangeException(nameof(action), action, null)
        };
    }
}
