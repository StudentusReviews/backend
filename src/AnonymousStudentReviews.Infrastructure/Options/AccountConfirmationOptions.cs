namespace AnonymousStudentReviews.Infrastructure.Options;

public class AccountConfirmationOptions
{
    public const string SectionName = "AccountConfirmation";

    public int EmailVerificationTokenExpirationHours { get; set; }
    public string SendConfirmationLetterFromAddress { get; set; } = string.Empty;
    public string ConfirmationLetterSubject { get; set; } = string.Empty;
}
