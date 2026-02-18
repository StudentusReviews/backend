namespace AnonymousStudentReviews.Api.Features.Universities.Update;

public class UpdateUniversityRequest
{
    public string Name { get; set; } = string.Empty;
    public string? City { get; set; }
    public string? Website { get; set; }
    public string? Description { get; set; }
}
