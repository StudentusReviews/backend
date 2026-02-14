using AnonymousStudentReviews.UseCases.AccountVerification;
<<<<<<< HEAD
<<<<<<< HEAD
using AnonymousStudentReviews.UseCases.ApplicationToAddAUniversity.Create;
using AnonymousStudentReviews.UseCases.ApplicationToAddAUniversity.Delete;
using AnonymousStudentReviews.UseCases.ApplicationToAddAUniversity.View;
using AnonymousStudentReviews.UseCases.Dummies.Create;
=======
>>>>>>> f5cb2cd (Add Delete and View controllers. Created configurations for AppToAddAUni and AppToAddAUniStatus)
using AnonymousStudentReviews.UseCases.AppToAddAUni.Create;
using AnonymousStudentReviews.UseCases.AppToAddAUni.Delete;
using AnonymousStudentReviews.UseCases.AppToAddAUni.View;
=======
using AnonymousStudentReviews.UseCases.ApplicationToAddAUniversity.Create;
using AnonymousStudentReviews.UseCases.ApplicationToAddAUniversity.Delete;
using AnonymousStudentReviews.UseCases.ApplicationToAddAUniversity.View;
>>>>>>> 7eca784 (Changed names of classes. AppToAddAUni -> ApplicationToAddAUniversity)
using AnonymousStudentReviews.UseCases.Dummies.Create;
using AnonymousStudentReviews.UseCases.Login;
using AnonymousStudentReviews.UseCases.Registration;
using AnonymousStudentReviews.UseCases.Reviews.Create;
using AnonymousStudentReviews.UseCases.Reviews.Create;
using AnonymousStudentReviews.UseCases.Reviews.Delete;
using AnonymousStudentReviews.UseCases.Reviews.Delete;
using AnonymousStudentReviews.UseCases.Reviews.Edit;
using AnonymousStudentReviews.UseCases.Reviews.Edit;
using AnonymousStudentReviews.UseCases.Universities.RetrieveMany;
using AnonymousStudentReviews.UseCases.Universities.RetrieveMany;
using AnonymousStudentReviews.UseCases.Universities.RetrieveOne;

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
        services.AddScoped<ICreateReviewService, CreateReviewService>();
        services.AddScoped<IEditReviewService, EditReviewService>();
        services.AddScoped<IDeleteReviewService, DeleteReviewService>();
        services.AddScoped<IRetrieveManyUniversitiesService, RetrieveManyUniversitiesService>();
        services.AddScoped<ICreateReviewService, CreateReviewService>();
        services.AddScoped<IEditReviewService, EditReviewService>();
        services.AddScoped<IDeleteReviewService, DeleteReviewService>();
        services.AddScoped<IRetrieveManyUniversitiesService, RetrieveManyUniversitiesService>();
        services.AddScoped<ICreateReviewService, CreateReviewService>();
        services.AddScoped<IEditReviewService, EditReviewService>();
        services.AddScoped<IDeleteReviewService, DeleteReviewService>();
        services.AddScoped<IRetrieveManyUniversitiesService, RetrieveManyUniversitiesService>();
        services.AddScoped<IRetrieveOneUniversityService, RetrieveOneUniversityService>();
        services.AddScoped<ICreateApplicationToAddAUniversityService, CreateApplicationToAddAUniversityService>();
        services.AddScoped<IDeleteApplicationToAddAUniversityService, DeleteApplicationToAddAUniversityService>();
        services.AddScoped<IViewApplicationToAddAUniversityService, ViewApplicationToAddAUniversityService>();
    }
}
