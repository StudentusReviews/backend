namespace AnonymousStudentReviews.UseCases.Users.Create;

public interface IEmailSender
{
    Task SendEmailAsync(string fromEmailAddress, IEnumerable<string> toEmailAddresses, string subject, string htmlBody);
}
