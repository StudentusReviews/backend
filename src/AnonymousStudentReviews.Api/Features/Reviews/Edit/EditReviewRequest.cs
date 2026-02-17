namespace AnonymousStudentReviews.Api.Features.Reviews.Edit;

public class EditReviewRequest
{
    public int Score { get; set; }
    public string Body { get; set; } = string.Empty;
}
