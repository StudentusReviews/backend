using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.AllowedEmailDomain;
using AnonymousStudentReviews.Core.Aggregates.ApplicationToAddAUniversity.Base;
using AnonymousStudentReviews.Core.Aggregates.ApplicationToAddAUniversity.Status;
using AnonymousStudentReviews.Core.Aggregates.Dummy;
using AnonymousStudentReviews.Core.Aggregates.EmailVerificationToken;
using AnonymousStudentReviews.Core.Aggregates.Review;
using AnonymousStudentReviews.Core.Aggregates.Role;
using AnonymousStudentReviews.Core.Aggregates.University;
using AnonymousStudentReviews.Core.Aggregates.User;
using AnonymousStudentReviews.Infrastructure.AllowedEmailDomains;
using AnonymousStudentReviews.Infrastructure.Applications;
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

using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Npgsql;

using Quartz;

using Resend;

namespace AnonymousStudentReviews.Infrastructure;

public static class InfrastructureServiceExtensions
{
    private static string GetConnectionString(
        IConfiguration configuration,
        string connectionStringName,
        string sectionPath)
    {
        var fromConnectionStrings = configuration.GetConnectionString(connectionStringName);
        if (!string.IsNullOrWhiteSpace(fromConnectionStrings))
        {
            return fromConnectionStrings;
        }

        var section = configuration.GetSection(sectionPath);
        if (!section.Exists())
        {
            throw new Exception(
                $"Neither ConnectionStrings:{connectionStringName} nor {sectionPath} section is configured.");
        }

        var host = section["Host"];
        var portRaw = section["Port"];
        var database = section["Database"];
        var username = section["Username"];
        var password = section["Password"];

        if (string.IsNullOrWhiteSpace(host) ||
            string.IsNullOrWhiteSpace(portRaw) ||
            string.IsNullOrWhiteSpace(database) ||
            string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(password))
        {
            throw new Exception($"Incomplete database config in {sectionPath}. Required: Host, Port, Database, Username, Password.");
        }

        if (!int.TryParse(portRaw, out var port))
        {
            throw new Exception($"Invalid Port value in {sectionPath}: '{portRaw}'.");
        }

        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = host,
            Port = port,
            Database = database,
            Username = username,
            Password = password
        };

        return builder.ConnectionString;
    }

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
        var connectionString = GetConnectionString(
            configuration,
            "MainDatabase",
            "DatabaseConnections:MainDatabase");
        services.AddDbContext<ApplicationDatabaseContext>(options =>
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

                var roles = new[] { RoleNameConstants.Student, RoleNameConstants.Admin };

                foreach (var role in roles)
                {
                    InsertRoleIfNotExists(role);
                }

                void InsertApplicationToAddAUniversityIfNotExists(string name)
                {
                    if (context.Set<ApplicationToAddAUniversityStatus>().FirstOrDefault(status => status.Name == name) is null)
                    {
                        context.Set<ApplicationToAddAUniversityStatus>().Add(new ApplicationToAddAUniversityStatus { Id = Guid.NewGuid(), Name = name });
                    }
                }

                var statuses = new[] { StatusNameConstants.Pending, StatusNameConstants.Approved, StatusNameConstants.Rejected };

                foreach (var status in statuses)
                {
                    InsertApplicationToAddAUniversityIfNotExists(status);
                }

                context.SaveChanges();
            });
        });
    }

    private static void AddDataProtectionDbContextWithPostgres(IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = GetConnectionString(
            configuration,
            "DataProtectionDatabase",
            "DatabaseConnections:DataProtectionDatabase");
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
<<<<<<< HEAD
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IUniversityRepository, UniversityRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IUniversityRepository, UniversityRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IUniversityRepository, UniversityRepository>();
        services.AddScoped<IApplicationRepository, ApplicationRepository>();
        services.AddScoped<IApplicationStatusRepository, ApplicationStatusRepository>();
=======
        services.AddScoped<IApplicationToAddAUniversityRepository, ApplicationToAddAUniversityRepository>();
        services.AddScoped<IApplicationToAddAUniversityStatusRepository, ApplicationToAddAUniversityStatusRepository>();
>>>>>>> fd43300 (Fixed logic for the DeleteByIdAsync method. Fixed formatting issues. Where necessary, method implementations have been replaced with Async)
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
