using System.ComponentModel.DataAnnotations;

namespace AnonymousStudentReviews.Infrastructure.Options;

public class ResendApiOptions
{
    public const string SectionName = "Resend";

    [Required(ErrorMessage = "Missing configuration value for 'Resend:Key'.")]
    public string Key { get; init; } = string.Empty;
}
