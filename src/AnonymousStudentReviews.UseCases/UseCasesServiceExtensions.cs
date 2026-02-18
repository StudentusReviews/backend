using AnonymousStudentReviews.UseCases.AccountVerification;
using AnonymousStudentReviews.UseCases.ApplicationToAddAUniversity.Create;
using AnonymousStudentReviews.UseCases.ApplicationToAddAUniversity.Delete;
using AnonymousStudentReviews.UseCases.ApplicationToAddAUniversity.View;
using AnonymousStudentReviews.UseCases.Dummies.Create;
using AnonymousStudentReviews.UseCases.Login;
using AnonymousStudentReviews.UseCases.Registration;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AnonymousStudentReviews.UseCases;

public static class UseCasesServiceExtensions
{
    public static IServiceCollection AddUseCasesServices(
        this IServiceCollection services,
        IConfiguration configuration,
        ILogger logger,
        string environmentName)
    {
        if (environmentName == "Development")
        {
            RegisterDevelopmentOnlyDependencies(services);
        }
        else if (environmentName == "Testing")
        {
            RegisterTestingOnlyDependencies(services);
        }
        else
        {
            RegisterProductionOnlyDependencies(services);
        }

        RegisterServices(services);

        logger.LogInformation("{Project} services registered", "UseCases");

        return services;
    }

    private static void RegisterProductionOnlyDependencies(IServiceCollection services)
    {
    }

    private static void RegisterTestingOnlyDependencies(IServiceCollection services)
    {
    }

    private static void RegisterDevelopmentOnlyDependencies(IServiceCollection services)
    {
    }

    private static void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<ICreateDummyService, CreateDummyService>();
        services.AddScoped<IRegistrationService, RegistrationService>();
        services.AddScoped<IAccountVerificationService, AccountVerificationService>();
        services.AddScoped<ILoginService, LoginService>();
        services.AddScoped<ICreateApplicationToAddAUniversityService, CreateApplicationToAddAUniversityService>();
        services.AddScoped<IDeleteApplicationToAddAUniversityService, DeleteApplicationToAddAUniversityService>();
        services.AddScoped<IViewAllApplicationsToAddAUniversityService, ViewAllApplicationToAddAUniversityService>();
        services.AddScoped<IViewByIdApplicationToAddAUniversityService, ViewByIdApplicationToAddAUniversityService>();
    }
}
