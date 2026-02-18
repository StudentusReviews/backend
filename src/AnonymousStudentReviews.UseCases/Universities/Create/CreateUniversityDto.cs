namespace AnonymousStudentReviews.UseCases.Universities.Create;

public class CreateUniversityDto
{
    public string Name { get; set; } = string.Empty;
    public string? City { get; set; }
    public string? Website { get; set; }
    public string? Description { get; set; }
}
