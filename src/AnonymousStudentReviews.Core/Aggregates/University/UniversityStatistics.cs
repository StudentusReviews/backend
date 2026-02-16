using AnonymousStudentReviews.Core.Abstractions;

namespace AnonymousStudentReviews.Core.Aggregates.University;

public class UniversityStatistics
{
    public Guid UniversityId { get; init; }
    public int TotalScoreSum { get; private set; }
    public int TotalReviewCount { get; private set; }
    public int Rank { get; private set; }
    
    public University? University { get; init; }

    public static Result<UniversityStatistics> Create(Guid universityId)
    {
        var result = new UniversityStatistics { UniversityId = universityId };

        if (universityId == Guid.Empty)
        {
            return Result.Failure<UniversityStatistics>(UniversityStatisticsErrors.UniversityIdNotSet);
        }

        return Result.Success(result);
    }

    public Result UpdateRank(int rank)
    {
        if (rank <= 0)
        {
            return Result.Failure(UniversityStatisticsErrors.InvalidRank);
        }

        Rank = rank;

        return Result.Success();
    }

    public Result AddScore(int score)
    {
        if (score is < 1 or > 10)
        {
            return Result.Failure(UniversityStatisticsErrors.ScoreOutOfRange);
        }

        TotalScoreSum += score;
        TotalReviewCount++;

        return Result.Success();
    }

    public double GetAverageScore()
    {
        if (TotalReviewCount == 0)
        {
            return 0;
        }

        return (double)TotalScoreSum / TotalReviewCount;
    }
}
