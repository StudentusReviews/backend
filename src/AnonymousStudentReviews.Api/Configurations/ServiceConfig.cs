using System.Net;

using AnonymousStudentReviews.Api.Features.AccountVerification;
using AnonymousStudentReviews.Api.Features.Dummies.Create;
using AnonymousStudentReviews.Infrastructure;
using AnonymousStudentReviews.Infrastructure.Options;
using AnonymousStudentReviews.UseCases;
using AnonymousStudentReviews.UseCases.Registration.Abstractions;

using FluentValidation;

using Microsoft.AspNetCore.HttpOverrides;

using IPNetwork = Microsoft.AspNetCore.HttpOverrides.IPNetwork;

namespace AnonymousStudentReviews.Api.Configurations;

public static class ServiceConfig
{
    public static IServiceCollection AddServiceConfigs(this IServiceCollection services,
        ILogger logger,
        WebApplicationBuilder builder)
    {
        var headerForwardingOptions = new HeaderForwardingOptions();
        builder.Configuration.GetSection(HeaderForwardingOptions.SectionName)
            .Bind(headerForwardingOptions);

        if (!builder.Environment.IsDevelopment())
        {
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.ForwardLimit = 3;

                var networks = headerForwardingOptions.KnownNetworks.Select(network =>
                    new IPNetwork(IPAddress.Parse(network.Prefix), network.PrefixLength));

                foreach (var network in networks)
                {
                    options.KnownNetworks.Add(network);
                }

                var proxies = headerForwardingOptions.KnownProxies
                    .Select(proxy => IPAddress.Parse(proxy.IpAddress));

                foreach (var proxy in proxies)
                {
                    options.KnownProxies.Add(proxy);
                }
            });
        }

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
