using System.Net;

using AnonymousStudentReviews.Api.Extensions;
using AnonymousStudentReviews.Infrastructure.Options;

using Microsoft.AspNetCore.HttpOverrides;

using IPNetwork = Microsoft.AspNetCore.HttpOverrides.IPNetwork;

namespace AnonymousStudentReviews.Api.Configurations;

public static class HeaderForwardingConfig
{
    public static IServiceCollection AddHeaderForwardingConfig(this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        var headerForwardingOptions =
            configuration.GetValidated<HeaderForwardingOptions>(HeaderForwardingOptions.SectionName);

        if (!environment.IsDevelopment())
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

        return services;
    }
}
