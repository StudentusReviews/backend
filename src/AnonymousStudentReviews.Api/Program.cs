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

builder.AddLoggerConfigs();

builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

builder.Services.AddSwaggerConfig();

builder.Services.AddOptionsConfig(appLogger, builder);

builder.Services.AddServiceConfigs(appLogger, builder);

var app = builder.Build();


if (app.Environment.IsDevelopment() && MigrationConfig.ShouldApplyMigrationsOnStartup(app.Configuration, appLogger))
{
    app.UseMigrations(appLogger);
}

app.UseAppMiddleware();

if (app.Environment.IsDevelopment())
{
    app.MapGet("/", () => Results.Redirect("/swagger"));
}

app.Run();

namespace AnonymousStudentReviews.Api
{
    public class Program
    {
    }
}
