namespace AnonymousStudentReviews.Core.Aggregates.University;

public class UniversityDetailedPreview
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; set; }
    public double AverageScore { get; init; }
    public int ReviewCount { get; init; }
    public string? City { get; init; }
    public string? Website { get; init; }
    public string? IconUrl { get; init; }
    public DateTime CreatedAt { get; init; }
}
