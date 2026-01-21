namespace AnonymousStudentReviews.Core;

public class User
{
    public Guid Id { get; set; }
    public Guid UniversityId { get; set; }
    public string EmailHash { get; set; }
    public bool EmailConfirmed { get; set; }
    public DateTime CreatedAt { get; set; }

    public University? University { get; set; }
    public ICollection<Role> Roles { get; set; }
}
