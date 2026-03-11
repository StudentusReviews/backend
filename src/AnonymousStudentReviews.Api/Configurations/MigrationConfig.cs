using AnonymousStudentReviews.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;

namespace AnonymousStudentReviews.Api.Configurations;

public static class MigrationConfig
{
    public static void UseMigrations(this IApplicationBuilder app, ILogger logger)
    {
        using var scope = app.ApplicationServices.CreateScope();
        using var mainDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDatabaseContext>();
        using var dataProtectionDbContext = scope.ServiceProvider.GetRequiredService<DataProtectionDatabaseContext>();

        mainDbContext.Database.Migrate();
        dataProtectionDbContext.Database.Migrate();

        logger.LogInformation("Migrations applied successfully");
    }

    public static bool ShouldApplyMigrationsOnStartup(this IConfiguration configuration, ILogger logger)
    {
        var applyMigrationsOnStartupString = configuration["Migrations:ApplyMigrationsOnStartup"];

        if (applyMigrationsOnStartupString is null)
        {
            logger.LogCritical("Migrations:ApplyMigrationsOnStartup not found in config file");
            return false;
        }

        var applyMigrationsOnStartup = bool.Parse(applyMigrationsOnStartupString);

        return applyMigrationsOnStartup;
    }
}
