namespace AnonymousStudentReviews.Infrastructure.Options;

public class LoginOptions
{
    public const string SectionName = "Login";

    public int MaxFailedAccessAttempts { get; set; }
    public int LockoutTimeMinutes { get; set; }
}
