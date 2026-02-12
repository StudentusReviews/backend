using System;

namespace AnonymousStudentReviews.Api.Features.Reviews.Common;

public class ReviewResponse
{
    public Guid Id { get; set; }
    public Guid UniversityId { get; set; }
    public Guid UserId { get; set; }
    public int Score { get; set; }
    public string Body { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
