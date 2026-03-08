using System.ComponentModel.DataAnnotations;

namespace AnonymousStudentReviews.Infrastructure.Options;

public class LoginOptions
{
    public const string SectionName = "Login";

    [Required(ErrorMessage = "Missing configuration value for 'Login:MaxFailedAccessAttempts'.")]
    [Range(1, int.MaxValue, ErrorMessage = "The value for 'Login:MaxFailedAccessAttempts' must be greater than 0.")]
    public int MaxFailedAccessAttempts { get; init; }

    [Required(ErrorMessage = "Missing configuration value for 'Login:LockoutTimeMinutes'.")]
    [Range(1, int.MaxValue, ErrorMessage = "The value for 'Login:LockoutTimeMinutes' must be greater than 0.")]
    public int LockoutTimeMinutes { get; init; }
}
