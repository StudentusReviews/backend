using System.Text.Json.Serialization;

using AnonymousStudentReviews.Api.Features.AccountVerification;
using AnonymousStudentReviews.Api.Features.Dummies.Create;
using AnonymousStudentReviews.Infrastructure;
using AnonymousStudentReviews.Infrastructure.Data;
using AnonymousStudentReviews.UseCases;
using AnonymousStudentReviews.UseCases.Registration.Abstractions;

using FluentValidation;

namespace AnonymousStudentReviews.Api.Configurations;

public static class ServiceConfig
{
    public static IServiceCollection AddServiceConfig(this IServiceCollection services,
        ILogger logger,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        services.AddHeaderForwardingConfig(configuration, environment);

        services
            .AddCorsConfig(configuration)
            .AddAuthenticationConfig()
            .AddAuthorization();

        services.AddControllersWithViews()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        services.AddRazorPages();

        services
            .AddSwaggerConfig()
            .AddOpenIddictConfig(environment, configuration)
            .AddExceptionHandler<GlobalExceptionHandler>()
            .AddProblemDetails()
            .AddHttpContextAccessor()
            .AddScoped<IAccountVerificationLinkFactory, AccountVerificationLinkFactory>()
            .AddInfrastructureServices(configuration, logger, environment.EnvironmentName)
            .AddUseCasesServices(configuration, logger, environment.EnvironmentName)
            .AddValidatorsFromAssemblyContaining<CreateDummyRequestValidator>();

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDatabaseContext>();

        return services;
    }
}
