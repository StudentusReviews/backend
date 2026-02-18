namespace AnonymousStudentReviews.UseCases.Universities.Update;

public class UpdateUniversityDto
{
    public Guid UniversityId { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? City { get; set; }
    public string? Website { get; set; }
    public string? Description { get; set; }
}
