using AnonymousStudentReviews.Infrastructure.Data;

using OpenIddict.Abstractions;

namespace AnonymousStudentReviews.Api.Configurations;

public static class OpenIddictConfig
{
    public static IServiceCollection AddOpenIddictConfig(this IServiceCollection services)
    {
        services.AddOpenIddict()
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore()
                    .UseDbContext<ApplicationDbContext>();
                options.UseQuartz();
            })
            .AddClient(options =>
            {
                options.AllowAuthorizationCodeFlow();


                options.AddDevelopmentEncryptionCertificate()
                    .AddDevelopmentSigningCertificate();


                options.UseAspNetCore()
                    .EnableStatusCodePagesIntegration()
                    .EnableRedirectionEndpointPassthrough();

                options.UseSystemNetHttp()
                    .SetProductInformation(typeof(Program).Assembly);

                // options.UseWebProviders()
                //     .AddGitHub(addGithubOptions =>
                //     {
                //         addGithubOptions.SetClientId("c4ade52327b01ddacff3")
                //             .SetClientSecret("da6bed851b75e317bf6b2cb67013679d9467c122")
                //             .SetRedirectUri("callback/login/github");
                //     });
            })
            .AddServer(options =>
            {
                options.SetAuthorizationEndpointUris("connect/authorize")
                    .SetEndSessionEndpointUris("connect/logout")
                    .SetTokenEndpointUris("connect/token")
                    .SetUserInfoEndpointUris("connect/userinfo");

                options.RegisterScopes(OpenIddictConstants.Permissions.Scopes.Email,
                    OpenIddictConstants.Permissions.Scopes.Profile, OpenIddictConstants.Permissions.Scopes.Roles);
                
                options.AllowAuthorizationCodeFlow();
                
                options.AddDevelopmentEncryptionCertificate()
                    .AddDevelopmentSigningCertificate();
                
                options.UseAspNetCore()
                    .EnableAuthorizationEndpointPassthrough()
                    .EnableEndSessionEndpointPassthrough()
                    .EnableTokenEndpointPassthrough()
                    .EnableUserInfoEndpointPassthrough()
                    .EnableStatusCodePagesIntegration();
            })
            
            .AddValidation(options =>
            {
                options.UseLocalServer();
                
                options.UseAspNetCore();
            });


        return services;
    }
}
