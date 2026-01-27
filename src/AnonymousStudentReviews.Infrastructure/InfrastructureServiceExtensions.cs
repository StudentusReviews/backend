using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.AllowedEmailDomain;
using AnonymousStudentReviews.Core.Aggregates.Dummy;
using AnonymousStudentReviews.Core.Aggregates.EmailVerificationToken;
using AnonymousStudentReviews.Core.Aggregates.Role;
using AnonymousStudentReviews.Core.Aggregates.User;
using AnonymousStudentReviews.Infrastructure.AllowedEmailDomains;
using AnonymousStudentReviews.Infrastructure.Data;
using AnonymousStudentReviews.Infrastructure.Dummies;
using AnonymousStudentReviews.Infrastructure.Email;
using AnonymousStudentReviews.Infrastructure.EmailVerificationToken;
using AnonymousStudentReviews.Infrastructure.Password;
using AnonymousStudentReviews.Infrastructure.Roles;
using AnonymousStudentReviews.Infrastructure.Users;
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
        RegisterServices(services, configuration);

        logger.LogInformation("{Project} services registered", "Infrastructure");

        return services;
    }

    private static void AddDbContextWithPostgres(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Postgres");
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
            options.UseSeeding((context, _) =>
            {
                void InsertRoleIfNotExists(string name)
                {
                    if (context.Set<Role>().FirstOrDefault(role => role.Name == "Student") is null)
                    {
                        context.Set<Role>().Add(new Role { Id = Guid.NewGuid(), Name = name });
                    }
                }

                var roles = new[] { "Student", "Admin" };

                foreach (var role in roles)
                {
                    InsertRoleIfNotExists(role);
                }

                context.SaveChanges();
            });
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
        services.AddScoped<IAllowedEmailDomainRepository, AllowedEmailDomainRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IEmailVerificationTokenRepository, EmailVerificationTokenRepository>();
    }

    private static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IEmailHasher, EmailHasher>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IEmailVerificationTokenHasher, EmailVerificationTokenHasher>();
        services.AddScoped<IEmailVerificationTokenGenerator, EmailVerificationTokenGenerator>();
        
    }
}
