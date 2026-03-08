namespace AnonymousStudentReviews.Core.Aggregates.University;

public class UniversityPreview
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public double AverageScore { get; init; }
    public int ReviewCount { get; init; }
    public string? City { get; init; }
    public string? Website { get; init; }
    public string? IconUrl { get; init; }
    public DateTime CreatedAt { get; init; }
}
