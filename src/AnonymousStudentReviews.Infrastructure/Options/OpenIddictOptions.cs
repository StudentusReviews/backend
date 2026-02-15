using OpenIddict.Abstractions;

namespace AnonymousStudentReviews.Infrastructure.Options;

public class OpenIddictOptions
{
    public const string SectionName = "OpenIddict";

    public List<OpenIddictApplicationOptions> Applications { get; set; } = new();
}

public class OpenIddictApplicationOptions
{
    public ClientTypeOption ClientType { get; set; }
    public string ClientId { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public ConsentTypeOption ConsentType { get; set; }
    public ApplicationTypeOption ApplicationType { get; set; }
    public List<string> RedirectUris { get; set; } = new();
    public List<string> PostLogoutRedirectUris { get; set; } = new();
    public ApplicationPermissionsOptions Permissions { get; set; } = new();
    public ApplicationRequirementsOptions Requirements { get; set; } = new();
}

public class ApplicationPermissionsOptions
{
    public List<EndpointOption> Endpoints { get; set; } = new();
    public List<GrantTypeOption> GrantTypes { get; set; } = new();
    public List<ScopeOption> Scopes { get; set; } = new();
    public List<ResponseTypeOption> ResponseTypes { get; set; } = new();
}

public class ApplicationRequirementsOptions
{
    public List<FeatureOption> Features { get; set; } = new();
}

public enum EndpointOption
{
    Authorization,
    DeviceAuthorization,
    EndSession,
    Introspection,
    PushedAuthorization,
    Revocation,
    Token
}

public static class EndpointOptionExtensions
{
    public static string ToOpenIddictEndpoint(this EndpointOption endpoint)
    {
        return endpoint switch
        {
            EndpointOption.Authorization => OpenIddictConstants.Permissions.Endpoints.Authorization,
            EndpointOption.DeviceAuthorization => OpenIddictConstants.Permissions.Endpoints.DeviceAuthorization,
            EndpointOption.EndSession => OpenIddictConstants.Permissions.Endpoints.EndSession,
            EndpointOption.Introspection => OpenIddictConstants.Permissions.Endpoints.Introspection,
            EndpointOption.PushedAuthorization => OpenIddictConstants.Permissions.Endpoints.PushedAuthorization,
            EndpointOption.Revocation => OpenIddictConstants.Permissions.Endpoints.Revocation,
            EndpointOption.Token => OpenIddictConstants.Permissions.Endpoints.Token,
            _ => throw new NotImplementedException($"Endpoint permission {endpoint} is not supported.")
        };
    }
}

public enum GrantTypeOption
{
    AuthorizationCode,
    ClientCredentials,
    DeviceCode,
    Implicit,
    Password,
    RefreshToken,
    TokenExchange
}

public static class GrantTypeOptionExtensions
{
    public static string ToOpenIddictGrantType(this GrantTypeOption grantType)
    {
        return grantType switch
        {
            GrantTypeOption.AuthorizationCode => OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
            GrantTypeOption.ClientCredentials => OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
            GrantTypeOption.DeviceCode => OpenIddictConstants.Permissions.GrantTypes.DeviceCode,
            GrantTypeOption.Implicit => OpenIddictConstants.Permissions.GrantTypes.Implicit,
            GrantTypeOption.Password => OpenIddictConstants.Permissions.GrantTypes.Password,
            GrantTypeOption.RefreshToken => OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
            GrantTypeOption.TokenExchange => OpenIddictConstants.Permissions.GrantTypes.TokenExchange,
            _ => throw new NotImplementedException($"Grant type permission {grantType} is not supported.")
        };
    }
}

public enum ResponseTypeOption
{
    Code,
    CodeIdToken,
    CodeIdTokenToken,
    CodeToken,
    IdToken,
    IdTokenToken,
    None,
    Token
}

public static class ResponseTypeOptionExtensions
{
    public static string ToOpenIddictResponseType(this ResponseTypeOption responseType)
    {
        return responseType switch
        {
            ResponseTypeOption.Code => OpenIddictConstants.Permissions.ResponseTypes.Code,
            ResponseTypeOption.CodeIdToken => OpenIddictConstants.Permissions.ResponseTypes.CodeIdToken,
            ResponseTypeOption.CodeIdTokenToken => OpenIddictConstants.Permissions.ResponseTypes.CodeIdTokenToken,
            ResponseTypeOption.CodeToken => OpenIddictConstants.Permissions.ResponseTypes.CodeToken,
            ResponseTypeOption.IdToken => OpenIddictConstants.Permissions.ResponseTypes.IdToken,
            ResponseTypeOption.IdTokenToken => OpenIddictConstants.Permissions.ResponseTypes.IdTokenToken,
            ResponseTypeOption.None => OpenIddictConstants.Permissions.ResponseTypes.None,
            ResponseTypeOption.Token => OpenIddictConstants.Permissions.ResponseTypes.Token,
            _ => throw new NotImplementedException($"Response type permission {responseType} is not supported.")
        };
    }
}

public enum ScopeOption
{
    Address,
    Email,
    Phone,
    Profile,
    Roles
}

public static class ScopeOptionExtensions
{
    public static string ToOpenIddictScope(this ScopeOption scope)
    {
        return scope switch
        {
            ScopeOption.Address => OpenIddictConstants.Permissions.Scopes.Address,
            ScopeOption.Email => OpenIddictConstants.Permissions.Scopes.Email,
            ScopeOption.Phone => OpenIddictConstants.Permissions.Scopes.Phone,
            ScopeOption.Profile => OpenIddictConstants.Permissions.Scopes.Profile,
            ScopeOption.Roles => OpenIddictConstants.Permissions.Scopes.Roles,
            _ => throw new NotImplementedException($"Scope permission {scope} is not supported.")
        };
    }
}

public enum FeatureOption
{
    ProofKeyForCodeExchange,
    PushedAuthorizationRequests
}

public static class FeatureOptionExtensions
{
    public static string ToOpenIddictFeature(this FeatureOption feature)
    {
        return feature switch
        {
            FeatureOption.ProofKeyForCodeExchange => OpenIddictConstants.Requirements.Features.ProofKeyForCodeExchange,
            FeatureOption.PushedAuthorizationRequests => OpenIddictConstants.Requirements.Features
                .PushedAuthorizationRequests,
            _ => throw new NotImplementedException($"Feature requirement {feature} is not supported.")
        };
    }
}


public enum ConsentTypeOption
{
    Explicit,
    External,
    Implicit,
    Systematic
}

public static class ConsentTypeOptionExtensions
{
    public static string ToOpenIddictConsentType(this ConsentTypeOption consentType)
    {
        return consentType switch
        {
            ConsentTypeOption.Explicit => OpenIddictConstants.ConsentTypes.Explicit,
            ConsentTypeOption.External => OpenIddictConstants.ConsentTypes.External,
            ConsentTypeOption.Implicit => OpenIddictConstants.ConsentTypes.Implicit,
            ConsentTypeOption.Systematic => OpenIddictConstants.ConsentTypes.Systematic,
            _ => throw new NotImplementedException($"Consent type {consentType} is not supported.")
        };
    }
}

public enum ApplicationTypeOption
{
    Native,
    Web
}

public static class ApplicationTypeOptionExtensions
{
    public static string ToOpenIddictApplicationType(this ApplicationTypeOption applicationType)
    {
        return applicationType switch
        {
            ApplicationTypeOption.Native => OpenIddictConstants.ApplicationTypes.Native,
            ApplicationTypeOption.Web => OpenIddictConstants.ApplicationTypes.Web,
            _ => throw new NotImplementedException($"Application type {applicationType} is not supported.")
        };
    }
}

public enum ClientTypeOption
{
    Confidential,
    Public
}

public static class ClientTypeOptionExtensions
{
    public static string ToOpenIddictClientType(this ClientTypeOption clientType)
    {
        return clientType switch
        {
            ClientTypeOption.Confidential => OpenIddictConstants.ClientTypes.Confidential,
            ClientTypeOption.Public => OpenIddictConstants.ClientTypes.Public,
            _ => throw new NotImplementedException($"Client type {clientType} is not supported.")
        };
    }
}
