namespace AnonymousStudentReviews.Core.Aggregates.University;

public class UniversityPreview
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? City { get; set; }
    public string? Website { get; set; }
    public DateTime CreatedAt { get; set; }
}
