using AnonymousStudentReviews.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;

namespace AnonymousStudentReviews.Api.Configurations;

public static class MigrationConfig
{
    public static void UseMigrations(this IApplicationBuilder app, ILogger logger)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        using ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        dbContext.Database.Migrate();

        logger.LogInformation("Migrations applied successfully");
    }

    public static bool ShouldApplyMigrationsOnStartup(IConfiguration configuration, ILogger logger)
    {
        string? applyMigrationsOnStartupString = configuration["Migrations:ApplyMigrationsOnStartup"];

        if (applyMigrationsOnStartupString is null)
        {
            logger.LogCritical("Migrations:ApplyMigrationsOnStartup not found in config file");
            return false;
        }

        bool applyMigrationsOnStartup = bool.Parse(applyMigrationsOnStartupString);

        return applyMigrationsOnStartup;
    }
}
