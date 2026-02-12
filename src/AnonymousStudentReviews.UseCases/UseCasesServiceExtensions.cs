using AnonymousStudentReviews.UseCases.AccountVerification;
using AnonymousStudentReviews.UseCases.Dummies.Create;
using AnonymousStudentReviews.UseCases.Login;
using AnonymousStudentReviews.UseCases.Registration;
using AnonymousStudentReviews.UseCases.Users.Ban;
using AnonymousStudentReviews.UseCases.Users.Retrieve.RetrieveMany;
using AnonymousStudentReviews.UseCases.Users.Retrieve.RetrieveOne;

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
        services.AddScoped<IBanUserService, BanUserService>();
        services.AddScoped<IRetrieveManyUsersService, RetrieveManyUsersService>();
        services.AddScoped<IRetrieveOneUserService, RetrieveOneUserService>();
    }
}
