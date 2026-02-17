using AnonymousStudentReviews.Core.Abstractions;

namespace AnonymousStudentReviews.UseCases.Reviews.Outbox.CreateMessage;

public static class CreateMessageInReviewOutboxErrors
{
    public static readonly Error AlreadyExists = new("CreateMessageInReviewOutbox.AlreadyExists", "");

    public static readonly Error MessageCantBeInsertedWithProcessedState =
        new("CreateMessageInReviewOutbox.MessageCantBeInsertedWithProcessedState", "");
}
