using AnonymousStudentReviews.Api.Extensions;
using AnonymousStudentReviews.Api.Options;

namespace AnonymousStudentReviews.Api.Configurations;

public static class CorsConfig
{
    public static IServiceCollection AddCorsConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var corsOptions = configuration.GetValidated<CorsOptions>(CorsOptions.SectionName);

        services.AddCors(options =>
        {
            options.AddPolicy(ApiConstants.CorsPolicyName,
                policy =>
                {
                    policy.WithOrigins(corsOptions.AllowedOrigins.ToArray())
                        .WithHeaders()
                        .WithMethods()
                        .AllowCredentials();
                });
        });

        return services;
    }
}
