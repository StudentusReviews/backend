using AnonymousStudentReviews.Infrastructure.OpenId;
using AnonymousStudentReviews.Infrastructure.Options;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using OpenIddict.Abstractions;

namespace AnonymousStudentReviews.Infrastructure;

public class Worker : IHostedService
{
    private readonly IConfiguration _configuration;
    private readonly IOpenIddictApplicationManager _openIddictApplicationManager;
    private readonly OpenIddictOptions _openIddictOptions;
    private readonly IOpenIddictScopeManager _openIddictScopeManager;

    public Worker(IOptions<OpenIddictOptions> openIddictOptions,
        IConfiguration configuration, IOpenIddictApplicationManager openIddictApplicationManager,
        IOpenIddictScopeManager openIddictScopeManager)
    {
        _configuration = configuration;
        _openIddictApplicationManager = openIddictApplicationManager;
        _openIddictScopeManager = openIddictScopeManager;
        _openIddictOptions = openIddictOptions.Value;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await RegisterScopesAsync(cancellationToken);
        await RegisterApplicationsAsync(cancellationToken);
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

    private async Task RegisterScopesAsync(CancellationToken cancellationToken)
    {
        var universityScopeDescriptor = new OpenIddictScopeDescriptor
        {
            Name = CustomOpenIdScopes.UniversityId, DisplayName = "University id"
        };

        var scopeExists = await _openIddictScopeManager
            .FindByNameAsync(universityScopeDescriptor.Name, cancellationToken) is not null;

        if (!scopeExists)
        {
            await _openIddictScopeManager.CreateAsync(universityScopeDescriptor, cancellationToken);
        }
    }

    private async Task RegisterApplicationsAsync(CancellationToken cancellationToken)
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
                await _openIddictApplicationManager.FindByClientIdAsync(openIddictApplicationOptions.ClientId,
                    cancellationToken);

            if (clientFromDatabase is null)
            {
                await _openIddictApplicationManager.CreateAsync(openIddictApplicationDescriptor, cancellationToken);
            }
            else
            {
                await _openIddictApplicationManager.UpdateAsync(clientFromDatabase, openIddictApplicationDescriptor,
                    cancellationToken);
            }
        }
    }
}
