using AnonymousStudentReviews.Api.Extensions;
using AnonymousStudentReviews.Api.Options;

namespace AnonymousStudentReviews.Api.Configurations;

public static class CorsConfig
{
    public static WebApplicationBuilder AddCorsConfig(this WebApplicationBuilder builder)
    {
        var corsOptions = builder.Configuration.GetValidated<CorsOptions>(CorsOptions.SectionName);

        builder.Services.AddCors(options =>
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

        return builder;
    }
}
