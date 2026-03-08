using AnonymousStudentReviews.UseCases.Registration.Abstractions;

using Resend;

namespace AnonymousStudentReviews.Infrastructure.Email;

public class EmailSender : IEmailSender
{
    private readonly IResend _resend;

    public EmailSender(IResend resend)
    {
        _resend = resend;
    }

    public async Task SendEmailAsync(string fromEmailAddress, IEnumerable<string> toEmailAddresses, string subject,
        string htmlBody)
    {
        var toEmailAddressList = new EmailAddressList();

        foreach (var toEmailAddress in toEmailAddresses)
        {
            toEmailAddressList.Add(toEmailAddress);
        }

        var message = new EmailMessage
        {
            From = fromEmailAddress,
            Subject = subject,
            HtmlBody = htmlBody,
            To = toEmailAddressList
        };

        await _resend.EmailSendAsync(message);
    }
}
