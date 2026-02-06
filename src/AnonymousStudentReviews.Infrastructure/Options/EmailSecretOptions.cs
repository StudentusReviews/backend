namespace AnonymousStudentReviews.Infrastructure.Options;

public class EmailSecretOptions
{
    public const string SectionName = "EmailSecrets";

    public string EmailVerificationTokenHashKey { get; set; } = string.Empty;
    public string EmailHashKey { get; set; } = string.Empty;
}
