using Serilog;

namespace AnonymousStudentReviews.Api.Configurations;

public static class LoggerConfig
{
    public static WebApplicationBuilder AddLoggerConfig(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog();

        return builder;
    }
}
