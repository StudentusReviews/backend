using AnonymousStudentReviews.UseCases.Users.Create;

using Resend;

namespace AnonymousStudentReviews.Infrastructure.Email;

public class EmailSender : IEmailSender
{
    private readonly IResend _resend;

    public EmailSender(IResend resend)
    {
        _resend = resend;
    }

    public async Task SendEmailAsync(string from, string to, string subject, string htmlBody)
    {
        var message = new EmailMessage
        {
            From = "Acme <onboarding@resend.dev>",
            Subject = subject,
            HtmlBody = htmlBody
        };
        message.To.Add( "delivered@resend.dev" );
        message.Subject = "hello world";
        message.HtmlBody = "<strong>it works!</strong>";

        await _resend.EmailSendAsync( message );
    }
}
