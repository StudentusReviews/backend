using AnonymousStudentReviews.Infrastructure.Data;
using AnonymousStudentReviews.Infrastructure.Options;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using OpenIddict.Abstractions;

using static OpenIddict.Abstractions.OpenIddictConstants;

namespace AnonymousStudentReviews.Infrastructure;

public class Worker : IHostedService
{
    private readonly OpenIddictOptions _openIddictOptions;
    private readonly IServiceProvider _serviceProvider;

    public Worker(IServiceProvider serviceProvider, IOptions<OpenIddictOptions> openIddictOptions)
    {
        _serviceProvider = serviceProvider;
        _openIddictOptions = openIddictOptions.Value;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();

        // var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        // await context.Database.EnsureCreatedAsync(cancellationToken);

        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

        var redirectUris = _openIddictOptions.RedirectUris
            .Select(redirectUri => new Uri(redirectUri));
        
        var postLogoutRedirectUris = _openIddictOptions.PostLogoutRedirectUris
            .Select(redirectUri => new Uri(redirectUri));

        if (await manager.FindByClientIdAsync(_openIddictOptions.ClientId, cancellationToken) == null)
        {
            var openIddictDescriptor = new OpenIddictApplicationDescriptor
            {
                ClientId = _openIddictOptions.ClientId,
                ConsentType = ConsentTypes.Implicit,
                DisplayName = _openIddictOptions.DisplayName,
                RedirectUris = {},
                PostLogoutRedirectUris = {},
                Permissions =
                {
                    Permissions.Endpoints.Authorization,
                    Permissions.Endpoints.EndSession,
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.AuthorizationCode,
                    Permissions.ResponseTypes.Code,
                    Permissions.Scopes.Email,
                    Permissions.Scopes.Profile,
                    Permissions.Scopes.Roles,
                },
                Requirements = { Requirements.Features.ProofKeyForCodeExchange }
            };

            foreach (var redirectUri in redirectUris)
            {
                openIddictDescriptor.RedirectUris.Add(redirectUri);
            }

            foreach (var postLogoutRedirectUri in postLogoutRedirectUris)
            {
                openIddictDescriptor.PostLogoutRedirectUris.Add(postLogoutRedirectUri);
            }

            await manager.CreateAsync(openIddictDescriptor, cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
