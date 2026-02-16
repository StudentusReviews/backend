using AnonymousStudentReviews.Core.Abstractions;

namespace AnonymousStudentReviews.Core.Aggregates.University;

public static class UniversityStatisticsErrors
{
    public static readonly Error UniversityIdNotSet = new("UniversityStatistics.UniversityIdNotSet", "");
    public static readonly Error InvalidRank = new("UniversityStatistics.InvalidRank", "");
    public static readonly Error ScoreOutOfRange = new("UniversityStatistics.ScoreOutOfRange", "");
}
