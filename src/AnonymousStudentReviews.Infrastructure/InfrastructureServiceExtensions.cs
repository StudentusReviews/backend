using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.AllowedEmailDomain;
using AnonymousStudentReviews.Core.Aggregates.Dummy;
using AnonymousStudentReviews.Infrastructure.Data;
using AnonymousStudentReviews.Infrastructure.Dummies;
using AnonymousStudentReviews.Infrastructure.Email;
using AnonymousStudentReviews.Infrastructure.Password;
using AnonymousStudentReviews.UseCases.Users.Create;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AnonymousStudentReviews.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration,
        ILogger logger,
        string environmentName)
    {
        if (environmentName == "Development")
        {
            RegisterDevelopmentOnlyDependencies(services, configuration);
        }
        else if (environmentName == "Testing")
        {
            RegisterTestingOnlyDependencies(services);
        }
        else
        {
            RegisterProductionOnlyDependencies(services, configuration);
        }

        RegisterEFRepositories(services);
        RegisterServices(services);

        logger.LogInformation("{Project} services registered", "Infrastructure");

        return services;
    }

    private static void AddDbContextWithPostgres(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Postgres");
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
    }


    private static void RegisterDevelopmentOnlyDependencies(IServiceCollection services, IConfiguration configuration)
    {
        AddDbContextWithPostgres(services, configuration);
    }

    private static void RegisterTestingOnlyDependencies(IServiceCollection services)
    {
    }

    private static void RegisterProductionOnlyDependencies(IServiceCollection services, IConfiguration configuration)
    {
        AddDbContextWithPostgres(services, configuration);
    }

    private static void RegisterEFRepositories(IServiceCollection services)
    {
        services.AddScoped<IDummyRepository, DummyRepository>();
        services.AddScoped<IAllowedEmailDomainRepository, IAllowedEmailDomainRepository>();
    }

    private static void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IEmailHasher, EmailHasher>();
        services.AddScoped<IAllowedEmailDomainValidator, AllowedEmailDomainValidator>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
    }
}
