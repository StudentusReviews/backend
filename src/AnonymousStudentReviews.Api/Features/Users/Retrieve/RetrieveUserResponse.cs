namespace AnonymousStudentReviews.Api.Features.Users.Retrieve;

public class RetrieveUserResponse
{
    public Guid UserId { get; set; }
    public Guid UniversityId { get; set; }
    public IEnumerable<string> Roles { get; }
    public string? UniversityName { get; set; }
}
