using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.Review;

namespace AnonymousStudentReviews.UseCases.Reviews.Outbox.CreateMessage;

public class CreateMessageInReviewOutboxService : ICreateMessageInReviewOutboxService
{
    private readonly IReviewOutboxRepository _reviewOutboxRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateMessageInReviewOutboxService(IReviewOutboxRepository reviewOutboxRepository, IUnitOfWork unitOfWork)
    {
        _reviewOutboxRepository = reviewOutboxRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(CreateMessageInReviewOutboxDto dto)
    {
        var pendingReviewOutboxState = await _reviewOutboxRepository
            .GetReviewOutboxStateEntityAsync(ReviewOutboxState.Pending);

        var payload = new ReviewOutboxPayload { Score = dto.Score, OldScore = dto.OldScore };

        var actionEntity = await _reviewOutboxRepository.GetReviewOutboxActionEntityAsync(dto.ReviewOutboxAction);

        var reviewOutbox = new ReviewOutbox
        {
            Id = Guid.NewGuid(),
            StateId = pendingReviewOutboxState.Id,
            ActionId = actionEntity.Id,
            Payload = payload
        };

        _reviewOutboxRepository.Create(reviewOutbox);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
