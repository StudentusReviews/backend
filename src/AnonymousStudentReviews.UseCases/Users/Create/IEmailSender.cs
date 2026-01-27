namespace AnonymousStudentReviews.UseCases.Users.Create;

public interface IEmailSender
{
    Task SendEmailAsync(string from, string to, string subject, string htmlBody);
}
