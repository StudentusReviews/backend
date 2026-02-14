using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.AllowedEmailDomain;
using AnonymousStudentReviews.Core.Aggregates.Dummy;
using AnonymousStudentReviews.Core.Aggregates.EmailVerificationToken;
using AnonymousStudentReviews.Core.Aggregates.Review;
using AnonymousStudentReviews.Core.Aggregates.Role;
using AnonymousStudentReviews.Core.Aggregates.University;
using AnonymousStudentReviews.Core.Aggregates.User;
using AnonymousStudentReviews.Infrastructure.AllowedEmailDomains;
using AnonymousStudentReviews.Infrastructure.Data;
using AnonymousStudentReviews.Infrastructure.Dummies;
using AnonymousStudentReviews.Infrastructure.Email;
using AnonymousStudentReviews.Infrastructure.EmailVerificationToken;
using AnonymousStudentReviews.Infrastructure.Options;
using AnonymousStudentReviews.Infrastructure.Password;
using AnonymousStudentReviews.Infrastructure.Reviews;
using AnonymousStudentReviews.Infrastructure.Roles;
using AnonymousStudentReviews.Infrastructure.Universities;
using AnonymousStudentReviews.Infrastructure.Users;
using AnonymousStudentReviews.UseCases.Abstractions;
using AnonymousStudentReviews.UseCases.Login.Abstractions;
using AnonymousStudentReviews.UseCases.Registration.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.Review;
using AnonymousStudentReviews.Infrastructure.Reviews;

using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Quartz;

using Resend;

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

    private static void AddMainDbContextWithPostgres(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MainDatabase");
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
            options.UseOpenIddict();
            options.UseSeeding((context, _) =>
            {
                void InsertRoleIfNotExists(string name)
                {
                    if (context.Set<Role>().FirstOrDefault(role => role.Name == name) is null)
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

    private static void AddDataProtectionDbContextWithPostgres(IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DataProtectionDatabase");
        services.AddDbContext<DataProtectionDatabaseContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
    }


    private static void RegisterDevelopmentOnlyDependencies(IServiceCollection services, IConfiguration configuration)
    {
        AddMainDbContextWithPostgres(services, configuration);
        AddDataProtectionDbContextWithPostgres(services, configuration);
    }

    private static void RegisterTestingOnlyDependencies(IServiceCollection services)
    {
    }

    private static void RegisterProductionOnlyDependencies(IServiceCollection services, IConfiguration configuration)
    {
        AddMainDbContextWithPostgres(services, configuration);
        AddDataProtectionDbContextWithPostgres(services, configuration);
    }

    private static void RegisterEFRepositories(IServiceCollection services)
    {
        services.AddScoped<IDummyRepository, DummyRepository>();
        services.AddScoped<IAllowedEmailDomainRepository, AllowedEmailDomainRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IEmailVerificationTokenRepository, EmailVerificationTokenRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IUniversityRepository, UniversityRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
    }

    private static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDataProtection()
            .PersistKeysToDbContext<DataProtectionDatabaseContext>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IEmailHasher, EmailHasher>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IEmailVerificationTokenHasher, EmailVerificationTokenHasher>();
        services.AddScoped<IEmailVerificationTokenGenerator, EmailVerificationTokenGenerator>();
        services.AddOptions();
        services.AddHttpClient<ResendClient>();

        var resendApiOptions = new ResendApiOptions();
        configuration.GetSection(ResendApiOptions.SectionName).Bind(resendApiOptions);

        services.Configure<ResendClientOptions>(o =>
        {
            o.ApiToken = resendApiOptions.Key;
        });

        services.AddTransient<IResend, ResendClient>();

        services.AddScoped<IEmailSender, EmailSender>();
        services.AddScoped<IUserManager, UserManager>();

        services.AddHostedService<Worker>();

        services.AddQuartz(options =>
        {
            options.UseSimpleTypeLoader();
            options.UseInMemoryStore();
        });

        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

        services.AddScoped<ISignInManager, SignInManager>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IViewToStringRenderer, ViewToStringRenderer>();
        services.AddScoped<IVerificationEmailGenerator, VerificationEmailGenerator>();
    }
}
