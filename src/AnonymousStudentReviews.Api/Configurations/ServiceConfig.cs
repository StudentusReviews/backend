using AnonymousStudentReviews.Api.Features.AccountVerification;
using AnonymousStudentReviews.Api.Features.Dummies.Create;
using AnonymousStudentReviews.Infrastructure;
using AnonymousStudentReviews.UseCases;
using AnonymousStudentReviews.UseCases.Registration.Abstractions;

using FluentValidation;

namespace AnonymousStudentReviews.Api.Configurations;

public static class ServiceConfig
{
    public static IServiceCollection AddServiceConfigs(this IServiceCollection services,
        ILogger logger,
        WebApplicationBuilder builder)
    {
        services
            .AddExceptionHandler<GlobalExceptionHandler>()
            .AddProblemDetails()
            .AddHttpContextAccessor()
            .AddScoped<IAccountVerificationLinkFactory, AccountVerificationLinkFactory>()
            .AddInfrastructureServices(builder.Configuration, logger, builder.Environment.EnvironmentName)
            .AddUseCasesServices(builder.Configuration, logger, builder.Environment.EnvironmentName)
            .AddValidatorsFromAssemblyContaining<CreateDummyRequestValidator>();

        return services;
    }
}
