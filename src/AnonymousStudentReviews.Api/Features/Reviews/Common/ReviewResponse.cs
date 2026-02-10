namespace AnonymousStudentReviews.Api.Features.Reviews.Common;

public class ReviewResponse
{
    public string Id { get; set; } = string.Empty;
    public string UniversityId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public int Score { get; set; }
    public string Body { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
