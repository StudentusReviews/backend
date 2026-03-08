using System.ComponentModel.DataAnnotations;

namespace AnonymousStudentReviews.Infrastructure.Options;

public class EmailSecretOptions
{
    public const string SectionName = "EmailSecrets";

    [Required(ErrorMessage = "Missing configuration value for 'EmailSecrets:EmailVerificationTokenHashKey'.")]
    public string EmailVerificationTokenHashKey { get; init; } = string.Empty;
    
    [Required(ErrorMessage = "Missing configuration value for 'EmailSecrets:EmailHashKey'.")]
    public string EmailHashKey { get; init; } = string.Empty;
}
