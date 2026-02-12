using AnonymousStudentReviews.Api;
using AnonymousStudentReviews.Api.Configurations;
using AnonymousStudentReviews.Api.Options;
using AnonymousStudentReviews.Infrastructure.Data;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;

using Serilog;
using Serilog.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>();

var corsOptions = new CorsOptions();
builder.Configuration.GetSection(CorsOptions.SectionName).Bind(corsOptions);

builder.Services.AddCors(options =>
{
    options.AddPolicy(ApiConstants.CorsPolicyName,
        policy =>
        {
            policy.WithOrigins(corsOptions.AllowedOrigins.ToArray())
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

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
    });

builder.AddLoggerConfigs();

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("/app/keys"));

builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

builder.Services.AddSwaggerConfig();

builder.Services.AddOptionsConfig(appLogger, builder);

builder.Services.AddServiceConfigs(appLogger, builder);

builder.Services.AddOpenIddictConfig();

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
