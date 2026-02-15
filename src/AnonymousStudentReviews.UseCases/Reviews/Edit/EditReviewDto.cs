namespace AnonymousStudentReviews.UseCases.Reviews.Edit;

public class EditReviewDto
{
    public Guid ReviewId { get; set; }
    public int Score { get; set; }
    public string Body { get; set; } = string.Empty;
}
