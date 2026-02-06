using System.Text.Json;

using AnonymousStudentReviews.Infrastructure.Options;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using OpenIddict.Abstractions;
using OpenIddict.EntityFrameworkCore.Models;

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

        var clientFromDatabase =
            await manager.FindByClientIdAsync(_openIddictOptions.ClientId, cancellationToken);

        var openIddictDescriptor = new OpenIddictApplicationDescriptor
        {
            ClientId = _openIddictOptions.ClientId,
            ConsentType = ConsentTypes.Implicit,
            DisplayName = _openIddictOptions.DisplayName,
            Permissions =
            {
                Permissions.Endpoints.Authorization,
                Permissions.Endpoints.EndSession,
                Permissions.Endpoints.Token,
                Permissions.GrantTypes.AuthorizationCode,
                Permissions.GrantTypes.RefreshToken,
                Permissions.ResponseTypes.Code,
                Permissions.Scopes.Email,
                Permissions.Scopes.Profile,
                Permissions.Scopes.Roles,
                Scopes.OfflineAccess
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

        if (clientFromDatabase is null)
        {
            await manager.CreateAsync(openIddictDescriptor, cancellationToken);
            return;
        }

        var clientFromDatabaseTyped = (OpenIddictEntityFrameworkCoreApplication)clientFromDatabase;

        clientFromDatabaseTyped.ClientId = openIddictDescriptor.ClientId;
        clientFromDatabaseTyped.DisplayName = openIddictDescriptor.DisplayName;

        var redirectUrisJson =
            JsonSerializer.Serialize(openIddictDescriptor.RedirectUris.ToArray());

        var postLogoutRedirectUrisJson =
            JsonSerializer.Serialize(openIddictDescriptor.PostLogoutRedirectUris.ToArray());

        var permissionsJson = JsonSerializer.Serialize(openIddictDescriptor.Permissions.ToArray());

        clientFromDatabaseTyped.Permissions = permissionsJson;

        clientFromDatabaseTyped.RedirectUris = redirectUrisJson;
        clientFromDatabaseTyped.PostLogoutRedirectUris = postLogoutRedirectUrisJson;

        await manager.UpdateAsync(clientFromDatabaseTyped, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
