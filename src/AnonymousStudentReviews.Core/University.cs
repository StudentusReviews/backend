using AnonymousStudentReviews.Core.Aggregates.AllowedEmailDomain;
using AnonymousStudentReviews.Core.Aggregates.User;

namespace AnonymousStudentReviews.Core;

public class University
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? City { get; set; }
    public string? Website { get; set; }

    public ICollection<User> Users { get; set; }
    public ICollection<AllowedEmailDomain> AllowedEmailDomains { get; set; }
}
