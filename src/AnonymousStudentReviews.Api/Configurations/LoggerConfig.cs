using Serilog;

namespace AnonymousStudentReviews.Api.Configurations;

public static class LoggerConfig
{
    public static WebApplicationBuilder AddLoggerConfig(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .WriteTo.Console());

        return builder;
    }
}
