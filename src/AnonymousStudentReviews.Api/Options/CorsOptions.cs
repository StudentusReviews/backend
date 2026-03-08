using System.ComponentModel.DataAnnotations;

namespace AnonymousStudentReviews.Api.Options;

public class CorsOptions
{
    public const string SectionName = "Cors";

    [Required(ErrorMessage = "Missing configuration value for 'Cors:AllowedOrigins'.")]
    [MinLength(1, ErrorMessage = "At least one allowed origin must be configured in 'Cors:AllowedOrigins'.")]
    public List<string> AllowedOrigins { get; init; } = new();
}
