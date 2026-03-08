using System.Text.Json.Serialization;

using AnonymousStudentReviews.Api.Configurations;
using AnonymousStudentReviews.Infrastructure.Data;

using Microsoft.AspNetCore.Authentication.Cookies;

using Serilog;
using Serilog.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDatabaseContext>();

builder.AddCorsConfig();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

Log.Information("Starting web host");

var loggerFactory = new SerilogLoggerFactory(Log.Logger);
var appLogger = loggerFactory.CreateLogger<AnonymousStudentReviews.Api.Program>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.LoginPath = "/api/login";
        options.LogoutPath = "/api/logout";
        options.ReturnUrlParameter = "return-url";
    });

builder.Services.AddAuthorization();

builder.AddLoggerConfig();

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddRazorPages();

builder.Services.AddSwaggerConfig();

builder.Services.AddServiceConfigs(appLogger, builder);

builder.Services.AddOpenIddictConfig(builder);

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
