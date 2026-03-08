using System.ComponentModel.DataAnnotations;

namespace AnonymousStudentReviews.Infrastructure.Options;

public class AccountConfirmationOptions
{
    public const string SectionName = "AccountConfirmation";

    [Required(ErrorMessage = "Missing configuration value for 'AccountConfirmation:EmailVerificationTokenExpirationMinutes'.")]
    [Range(1, int.MaxValue, ErrorMessage = "The value for 'AccountConfirmation:EmailVerificationTokenExpirationMinutes' must be greater than 0.")]
    public int EmailVerificationTokenExpirationMinutes { get; init; }

    [Required(ErrorMessage = "Missing configuration value for 'AccountConfirmation:SendConfirmationLetterFromAddress'.")]
    [EmailAddress(ErrorMessage = "The value for 'AccountConfirmation:SendConfirmationLetterFromAddress' must be a valid email address.")]
    public string SendConfirmationLetterFromAddress { get; init; } = string.Empty;

    [Required(ErrorMessage = "Missing configuration value for 'AccountConfirmation:ConfirmationLetterSubject'.")]
    public string ConfirmationLetterSubject { get; init; } = string.Empty;
}
