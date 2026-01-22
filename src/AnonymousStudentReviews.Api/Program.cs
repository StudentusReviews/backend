using AnonymousStudentReviews.Api.Configurations;

using Serilog;
using Serilog.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

logger.Information("Starting web host");

builder.AddLoggerConfigs();

var appLogger = new SerilogLoggerFactory(logger)
    .CreateLogger<AnonymousStudentReviews.Api.Program>();

builder.Services.AddControllers();

builder.Services.AddSwaggerConfig();

builder.Services.AddServiceConfigs(appLogger, builder);

var app = builder.Build();


if (app.Environment.IsDevelopment() && MigrationConfig.ShouldApplyMigrationsOnStartup(app.Configuration, appLogger))
{
    app.UseMigrations(appLogger);
}

app.UseAppMiddleware();

app.Run();

namespace AnonymousStudentReviews.Api
{
    public partial class Program
    {
    }
}
