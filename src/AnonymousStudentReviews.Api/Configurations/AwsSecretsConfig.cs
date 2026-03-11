using Microsoft.Extensions.Options;

namespace AnonymousStudentReviews.Api.Configurations;

public static class AwsSecretsConfig
{
    public static WebApplicationBuilder AddAwsSecretsConfig(this WebApplicationBuilder builder)
    {
        var awsSecretPath = builder.Configuration["AwsSecretPath"];

        if (string.IsNullOrEmpty(awsSecretPath))
        {
            throw new OptionsValidationException("AwsSecretPath", typeof(string),
                ["Missing configuration value for 'AwsSecretPath'."]);
        }

        builder.Configuration.AddSystemsManager(source =>
        {
            source.Path = awsSecretPath;

            // source.ReloadAfter = TimeSpan.FromMinutes(5);
        });

        return builder;
    }

    public static bool ShouldUseAwsSecrets(this IConfiguration configuration, ILogger logger)
    {
        var shouldUseAwsSecretsString = configuration["Secrets:UseAws"];

        if (string.IsNullOrEmpty(shouldUseAwsSecretsString))
        {
            logger.LogCritical("Secrets:UseAws not found in config file");
            return false;
        }

        var shouldUseAwsSecrets = bool.Parse(shouldUseAwsSecretsString);

        return shouldUseAwsSecrets;
    }
}
