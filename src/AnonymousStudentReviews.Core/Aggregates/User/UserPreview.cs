namespace AnonymousStudentReviews.Core.Aggregates.User;

public class UserPreview
{
    public Guid UserId { get; set; }
    public Guid? UniversityId { get; set; }
    public string? UniversityName { get; set; }
}
