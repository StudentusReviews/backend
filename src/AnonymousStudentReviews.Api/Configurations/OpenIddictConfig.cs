using System.Security.Cryptography.X509Certificates;

using AnonymousStudentReviews.Api.Extensions;
using AnonymousStudentReviews.Api.Options;
using AnonymousStudentReviews.Infrastructure.Data;
using AnonymousStudentReviews.Infrastructure.OpenId;

using OpenIddict.Abstractions;

namespace AnonymousStudentReviews.Api.Configurations;

public static class OpenIddictConfig
{
    public static IServiceCollection AddOpenIddictConfig(this IServiceCollection services,
        IWebHostEnvironment environment, IConfiguration configuration)
    {
        services.AddOpenIddict()
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore()
                    .UseDbContext<ApplicationDatabaseContext>();
                options.UseQuartz();
            })
            .AddServer(options =>
            {
                options.SetAuthorizationEndpointUris("connect/authorize")
                    .SetEndSessionEndpointUris("connect/logout")
                    .SetTokenEndpointUris("connect/token")
                    .SetUserInfoEndpointUris("connect/userinfo");

                options.RegisterScopes(
                    OpenIddictConstants.Permissions.Scopes.Email,
                    OpenIddictConstants.Permissions.Scopes.Profile,
                    OpenIddictConstants.Permissions.Scopes.Roles,
                    OpenIddictConstants.Scopes.Profile,
                    OpenIddictConstants.Scopes.Roles,
                    OpenIddictConstants.Scopes.OfflineAccess,
                    CustomOpenIdScopes.UniversityId
                );

                options.RegisterPromptValues(OpenIddictConstants.PromptValues.Login,
                    OpenIddictConstants.PromptValues.Create, OpenIddictConstants.PromptValues.Consent);

                options.AllowAuthorizationCodeFlow().AllowRefreshTokenFlow();

                if (environment.IsDevelopment())
                {
                    options.AddDevelopmentEncryptionCertificate()
                        .AddDevelopmentSigningCertificate();
                }

                if (!environment.IsDevelopment())
                {
                    var openIddictCertificateOptions =
                        configuration.GetValidated<OpenIddictCertificateOptions>(CorsOptions.SectionName);

                    var encryptionCert =
                        X509CertificateLoader.LoadPkcs12FromFile(
                            openIddictCertificateOptions.EncryptionCertificateFileContainerPath,
                            openIddictCertificateOptions.EncryptionCertificatePassword);

                    options.AddEncryptionCertificate(encryptionCert);

                    var signingCert =
                        X509CertificateLoader.LoadPkcs12FromFile(
                            openIddictCertificateOptions.SigningCertificateFileContainerPath,
                            openIddictCertificateOptions.SigningCertificatePassword);

                    options.AddSigningCertificate(signingCert);
                }


                options.UseAspNetCore()
                    .EnableAuthorizationEndpointPassthrough()
                    .EnableEndSessionEndpointPassthrough()
                    .EnableTokenEndpointPassthrough()
                    .EnableUserInfoEndpointPassthrough()
                    .EnableStatusCodePagesIntegration()
                    .DisableTransportSecurityRequirement();
            })
            .AddValidation(options =>
            {
                options.UseLocalServer();

                options.UseAspNetCore();
            });

        return services;
    }
}
