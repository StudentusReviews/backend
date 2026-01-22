using AnonymousStudentReviews.Core.Aggregates.User;

namespace AnonymousStudentReviews.Core;

public class Role
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public ICollection<User> Users { get; set; }
}
