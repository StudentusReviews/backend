using AnonymousStudentReviews.Api.Configurations;

using Serilog;
using Serilog.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

Log.Information("Starting web host");

var loggerFactory = new SerilogLoggerFactory(Log.Logger);
var appLogger = loggerFactory.CreateLogger<AnonymousStudentReviews.Api.Program>();

builder.AddLoggerConfig();

builder.Services.AddServiceConfig(appLogger, builder.Configuration, builder.Environment);

var app = builder.Build();

if (app.Environment.IsDevelopment() && MigrationConfig.ShouldApplyMigrationsOnStartup(app.Configuration, appLogger))
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
