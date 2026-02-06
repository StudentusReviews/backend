namespace AnonymousStudentReviews.Core.Aggregates.AllowedEmailDomain;

public class AllowedEmailDomain
{
    public Guid Id { get; set; }
    public Guid UniversityId { get; set; }
    public string Domain { get; set; }

    public University University { get; set; }
}
