using AnonymousStudentReviews.Infrastructure.Options;

namespace AnonymousStudentReviews.Api.Configurations;

public static class OptionsConfig
{
    public static IServiceCollection AddOptionsConfig(this IServiceCollection services,
        ILogger logger,
        WebApplicationBuilder builder)
    {
        services.Configure<EmailSecretOptions>(
            builder.Configuration.GetSection(EmailSecretOptions.SectionName));
        services.Configure<ResendApiOptions>(
            builder.Configuration.GetSection(ResendApiOptions.SectionName));
        services.Configure<AccountConfirmationOptions>(
            builder.Configuration.GetSection(AccountConfirmationOptions.SectionName));

        logger.LogInformation("Options registered successfully");

        return services;
    }
}
