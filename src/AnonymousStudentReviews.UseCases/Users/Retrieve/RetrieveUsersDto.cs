namespace AnonymousStudentReviews.UseCases.Users.Retrieve;

public class RetrieveUsersDto
{
    public string? QueryString { get; set; }
    public Guid? UserId { get; set; }
    public Guid? UniversityId { get; set; }
    public string? UniversityName { get; set; }
    public string? Email { get; set; }
    public int PageNumber { get; set; } = 0;
    public int PageSize { get; set; } = 10;
}
