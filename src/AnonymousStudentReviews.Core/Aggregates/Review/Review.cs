using AnonymousStudentReviews.Core.Abstractions;

namespace AnonymousStudentReviews.Core.Aggregates.Review;

public class Review
{
    public Guid Id { get; set; }
    public Guid UniversityId { get; set; }
    public Guid UserId { get; set; }
    public int Score { get; set; }
    public string Body { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public University.University? University { get; init; }

    public static Result<Review> Create(Guid universityId, Guid userId, int score, string body)
    {
        if (universityId == Guid.Empty || userId == Guid.Empty)
        {
            return Result.Failure<Review>(ReviewErrors.InvalidIds);
        }

        if (score is < 1 or > 10)
        {
            return Result.Failure<Review>(ReviewErrors.InvalidScore);
        }

        var trimmedBody = body?.Trim() ?? string.Empty;

        if (trimmedBody.Length < 1 || trimmedBody.Length > 4000)
        {
            return Result.Failure<Review>(ReviewErrors.InvalidBodyLength);
        }

        var now = DateTime.UtcNow;

        return Result.Success(new Review
        {
            Id = Guid.NewGuid(),
            UniversityId = universityId,
            UserId = userId,
            Score = score,
            Body = trimmedBody,
            CreatedAt = now,
            UpdatedAt = now
        });
    }

    public Result Update(int score, string body)
    {
        if (score is < 1 or > 10)
        {
            return Result.Failure(ReviewErrors.InvalidScore);
        }

        var trimmedBody = body?.Trim() ?? string.Empty;

        if (trimmedBody.Length < 1 || trimmedBody.Length > 4000)
        {
            return Result.Failure(ReviewErrors.InvalidBodyLength);
        }

        Score = score;
        Body = trimmedBody;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }
}
