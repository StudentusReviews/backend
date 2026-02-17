namespace AnonymousStudentReviews.Core.Aggregates.University;

public class University
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? City { get; set; }
    public string? Website { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<User.User>? Users { get; set; }
    public ICollection<AllowedEmailDomain.AllowedEmailDomain>? AllowedEmailDomains { get; set; }
}
