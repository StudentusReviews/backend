namespace AnonymousStudentReviews.Core.Aggregates.Role;

public class Role
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public ICollection<User.User> Users { get; set; }
}
