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
        if (dto.ReviewOutboxState == ReviewOutboxState.Processed)
        {
            return Result.Failure(CreateMessageInReviewOutboxErrors.MessageCantBeInsertedWithProcessedState);
        }

        var reviewOutboxState = await _reviewOutboxRepository
            .GetReviewOutboxStateEntityAsync(dto.ReviewOutboxState);

        if (await _reviewOutboxRepository.ReviewOutboxExistsAsync(reviewOutboxState, dto.Review))
        {
            return Result.Failure(CreateMessageInReviewOutboxErrors.AlreadyExists);
        }

        var reviewOutbox = new ReviewOutbox
        {
            Id = Guid.NewGuid(), ReviewId = dto.Review.Id, StateId = reviewOutboxState.Id
        };

        _reviewOutboxRepository.Create(reviewOutbox);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
