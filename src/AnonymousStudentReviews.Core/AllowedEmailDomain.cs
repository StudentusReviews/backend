namespace AnonymousStudentReviews.Core;

public class AllowedEmailDomain
{
    public Guid Id { get; set; }
    public Guid UniversityId { get; set; }
    public string Domain { get; set; }
}
