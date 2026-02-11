namespace AnonymousStudentReviews.Api.Features.Reviews.Create;

public class CreateReviewRequest
{
    public Guid UniversityId { get; set; }
    public int Score { get; set; }
    public string Body { get; set; } = string.Empty;
}
