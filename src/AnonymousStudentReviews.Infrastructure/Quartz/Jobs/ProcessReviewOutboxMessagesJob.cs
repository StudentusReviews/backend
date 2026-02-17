using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.Review;
using AnonymousStudentReviews.Core.Aggregates.University;
using AnonymousStudentReviews.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;

using Quartz;

namespace AnonymousStudentReviews.Infrastructure.Quartz.Jobs;

[DisallowConcurrentExecution]
public class ProcessReviewOutboxMessagesJob : IJob
{
    public static readonly JobKey Key = new("process-review-outbox-messages-job", "review-outbox");

    private readonly ApplicationDatabaseContext _databaseContext;
    private readonly IUnitOfWork _unitOfWork;

    public ProcessReviewOutboxMessagesJob(ApplicationDatabaseContext databaseContext, IUnitOfWork unitOfWork)
    {
        _databaseContext = databaseContext;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var processedState =
            await _databaseContext.ReviewOutboxStates.FirstOrDefaultAsync(e =>
                e.Name == ReviewOutboxState.Processed.GetName());

        var pendingState =
            await _databaseContext.ReviewOutboxStates.FirstOrDefaultAsync(e =>
                e.Name == ReviewOutboxState.Pending.GetName());

        var addAction = await _databaseContext.ReviewOutboxActions.FirstOrDefaultAsync(e =>
            e.Name == ReviewOutboxAction.Add.GetName());

        var updateAction = await _databaseContext.ReviewOutboxActions.FirstOrDefaultAsync(e =>
            e.Name == ReviewOutboxAction.Update.GetName());

        var deleteAction = await _databaseContext.ReviewOutboxActions.FirstOrDefaultAsync(e =>
            e.Name == ReviewOutboxAction.Delete.GetName());


        if (processedState is null || pendingState is null)
        {
            throw new Exception("Outbox states not found, seeding issue");
        }

        if (addAction is null || updateAction is null || deleteAction is null)
        {
            throw new Exception("Outbox actions not found, seeding issue");
        }

        var unprocessedOutboxMessages = await _databaseContext
            .ReviewOutbox
            .Where(e => e.StateId != processedState.Id)
            .Take(100)
            .ToListAsync();

        foreach (var unprocessedOutboxMessage in unprocessedOutboxMessages)
        {
            var universityStatistics =
                await GetOrCreateUniversityStatisticsAsync(unprocessedOutboxMessage.UniversityId);

            var actionId = unprocessedOutboxMessage.ActionId;

            if (actionId == addAction.Id)
            {
                universityStatistics.AddScore(unprocessedOutboxMessage.Payload.Score);
            }
            else if (actionId == updateAction.Id)
            {
                universityStatistics.UpdateScore(unprocessedOutboxMessage.Payload.OldScore,
                    unprocessedOutboxMessage.Payload.Score);
            }
            else if (actionId == deleteAction.Id)
            {
                universityStatistics.RemoveScore(unprocessedOutboxMessage.Payload.Score);
            }
        }

        await _unitOfWork.SaveChangesAsync();
    }

    private async Task<UniversityStatistics> GetOrCreateUniversityStatisticsAsync(Guid universityId)
    {
        var universityStatistics = await _databaseContext
            .UniversityStatistics
            .FindAsync(universityId);

        if (universityStatistics is null)
        {
            var createUniversityStatisticsResult =
                UniversityStatistics.Create(universityId);

            if (createUniversityStatisticsResult.IsFailure)
            {
                throw new Exception("University statistics university id can't be null");
            }

            var newUniversityStatistics = createUniversityStatisticsResult.Value;

            _databaseContext.UniversityStatistics.Add(newUniversityStatistics);

            await _unitOfWork.SaveChangesAsync();

            return newUniversityStatistics;
        }

        return universityStatistics!;
    }
}
