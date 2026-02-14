namespace AnonymousStudentReviews.Core.Aggregates.User;

public class User
{
    public Guid Id { get; set; }
    public Guid? UniversityId { get; set; }
    public string EmailHash { get; set; }
    public string PasswordHash { get; set; }
    public bool EmailConfirmed { get; set; }
    public int AccessFailedCount { get; set; }
    public DateTimeOffset? LockoutEnd { get; set; }
    public bool IsBanned { get; set; }
    public DateTime CreatedAt { get; set; }

    public University.University? University { get; set; }
    public ICollection<Role.Role> Roles { get; set; }
    public ICollection<ApplicationToAddAUniversity.Base.ApplicationToAddAUniversity> AppToAddAUnis { get; set; }
}
