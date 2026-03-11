using AnonymousStudentReviews.Api.Configurations;

using Serilog;
using Serilog.Extensions.Logging;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting web host");

var builder = WebApplication.CreateBuilder(args);

builder.AddLoggerConfig();

var appLogger = new SerilogLoggerFactory(Log.Logger).CreateLogger<Program>();

if (!builder.Environment.IsDevelopment() && builder.Configuration.ShouldUseAwsSecrets(appLogger))
{
    builder.AddAwsSecretsConfig();
}

builder.Services.AddServiceConfig(appLogger, builder.Configuration, builder.Environment);

var app = builder.Build();

if (app.Environment.IsDevelopment() && app.Configuration.ShouldApplyMigrationsOnStartup(appLogger))
{
    app.UseMigrations(appLogger);
}

app.UseAppMiddleware();

var healthCheckBuilder = app.MapHealthChecks("/healthz");

if (app.Environment.IsProduction())
{
    var requiredHost = app.Configuration["Healthcheck:RequireHost"];

    if (requiredHost is null)
    {
        throw new Exception("Healthcheck:RequireHost not set in config file");
    }

    healthCheckBuilder.RequireHost();
}

app.Run();

namespace AnonymousStudentReviews.Api
{
    public class Program
    {
    }
}
