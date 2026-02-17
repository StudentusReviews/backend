using AnonymousStudentReviews.Core.Abstractions;

namespace AnonymousStudentReviews.UseCases.Reviews.Outbox.CreateMessage;

public interface ICreateMessageInReviewOutboxService
{
    Task<Result> HandleAsync(CreateMessageInReviewOutboxDto dto);
}
