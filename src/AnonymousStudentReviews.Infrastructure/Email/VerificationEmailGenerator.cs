namespace AnonymousStudentReviews.Infrastructure.Email;

public class VerificationEmailGenerator : IVerificationEmailGenerator
{
    private readonly IViewToStringRenderer _viewToStringRenderer;

    public VerificationEmailGenerator(IViewToStringRenderer viewToStringRenderer)
    {
        _viewToStringRenderer = viewToStringRenderer;
    }

    public async Task<string> GenerateVerificationEmailAsync(string verificationLink)
    {
        var model = new VerificationEmailModel { VerificationLink = verificationLink };

        var email = await _viewToStringRenderer.RenderViewToStringAsync("/Views/AccountVerification/EmailTemplate.cshtml", model);

        return email;
    }
}
