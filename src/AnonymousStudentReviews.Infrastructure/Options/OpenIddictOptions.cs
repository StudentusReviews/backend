namespace AnonymousStudentReviews.Infrastructure.Options;

public class OpenIddictOptions
{
    public const string SectionName = "OpenIddict";

    public string ClientId { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public List<string> RedirectUris { get; set; } = new();
    public List<string> PostLogoutRedirectUris { get; set; } = new();
}
