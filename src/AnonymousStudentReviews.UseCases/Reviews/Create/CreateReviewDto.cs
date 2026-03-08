namespace AnonymousStudentReviews.UseCases.Reviews.Create;

public class CreateReviewDto
{
    public Guid UniversityId { get; set; }
    public int Score { get; set; }
    public string Body { get; set; } = string.Empty;
}
