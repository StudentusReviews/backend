using AnonymousStudentReviews.Infrastructure.OpenId;
using AnonymousStudentReviews.Infrastructure.Options;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using OpenIddict.Abstractions;

namespace AnonymousStudentReviews.Infrastructure;

public class Worker : IHostedService
{
    private readonly IConfiguration _configuration;
    private readonly OpenIddictOptions _openIddictOptions;
    private readonly IServiceProvider _serviceProvider;

    public Worker(IOptions<OpenIddictOptions> openIddictOptions,
        IConfiguration configuration, IServiceProvider serviceProvider)
    {
        _configuration = configuration;
        _serviceProvider = serviceProvider;
        _openIddictOptions = openIddictOptions.Value;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();

        var openIddictApplicationManager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
        var openIddictScopeManager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();
        
        await RegisterScopesAsync(cancellationToken, openIddictScopeManager);
        await RegisterApplicationsAsync(cancellationToken, openIddictApplicationManager);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private string GetClientSecretByClientId(string clientId)
    {
        var secret = _configuration[$"OpenIddictApplicationSecrets:{clientId}"];

        if (secret is null)
        {
            throw new Exception($"Secret for application with id {clientId} not set");
        }

        return secret;
    }

    private async Task RegisterScopesAsync(CancellationToken cancellationToken, IOpenIddictScopeManager scopeManager)
    {
        var universityScopeDescriptor = new OpenIddictScopeDescriptor
        {
            Name = CustomOpenIdScopes.UniversityId,
            DisplayName = "University id"
        };

        var scopeExists = await scopeManager
            .FindByNameAsync(universityScopeDescriptor.Name, cancellationToken) is not null;

        if (!scopeExists)
        {
            await scopeManager.CreateAsync(universityScopeDescriptor, cancellationToken);
        }
    }

    private async Task RegisterApplicationsAsync(CancellationToken cancellationToken, IOpenIddictApplicationManager applicationManager)
    {
        foreach (var openIddictApplicationOptions in _openIddictOptions.Applications)
        {
            var openIddictApplicationDescriptor = new OpenIddictApplicationDescriptor
            {
                ApplicationType = openIddictApplicationOptions.ApplicationType.ToOpenIddictApplicationType(),
                ClientType = openIddictApplicationOptions.ClientType.ToOpenIddictClientType(),
                ClientSecret = GetClientSecretByClientId(openIddictApplicationOptions.ClientId),
                ClientId = openIddictApplicationOptions.ClientId,
                ConsentType = openIddictApplicationOptions.ConsentType.ToOpenIddictConsentType(),
                DisplayName = openIddictApplicationOptions.DisplayName
            };

            foreach (var endpointOption in openIddictApplicationOptions.Permissions.Endpoints)
            {
                openIddictApplicationDescriptor.Permissions.Add(endpointOption.ToOpenIddictEndpoint());
            }

            foreach (var grantTypeOption in openIddictApplicationOptions.Permissions.GrantTypes)
            {
                openIddictApplicationDescriptor.Permissions.Add(grantTypeOption.ToOpenIddictGrantType());
            }

            foreach (var responseTypeOption in openIddictApplicationOptions.Permissions.ResponseTypes)
            {
                openIddictApplicationDescriptor.Permissions.Add(responseTypeOption.ToOpenIddictResponseType());
            }

            foreach (var scopeOption in openIddictApplicationOptions.Permissions.Scopes)
            {
                openIddictApplicationDescriptor.Permissions.Add(scopeOption.ToOpenIddictScope());
            }

            foreach (var featureOption in openIddictApplicationOptions.Requirements.Features)
            {
                openIddictApplicationDescriptor.Requirements.Add(featureOption.ToOpenIddictFeature());
            }

            foreach (var redirectUri in openIddictApplicationOptions.RedirectUris)
            {
                openIddictApplicationDescriptor.RedirectUris.Add(new Uri(redirectUri));
            }

            foreach (var postLogoutRedirectUri in openIddictApplicationOptions.PostLogoutRedirectUris)
            {
                openIddictApplicationDescriptor.PostLogoutRedirectUris.Add(new Uri(postLogoutRedirectUri));
            }

            var clientFromDatabase =
                await applicationManager.FindByClientIdAsync(openIddictApplicationOptions.ClientId,
                    cancellationToken);

            if (clientFromDatabase is null)
            {
                await applicationManager.CreateAsync(openIddictApplicationDescriptor, cancellationToken);
            }
            else
            {
                await applicationManager.UpdateAsync(clientFromDatabase, openIddictApplicationDescriptor,
                    cancellationToken);
            }
        }
    }
}
