using System.Security.Cryptography.X509Certificates;

using AnonymousStudentReviews.Infrastructure.Data;
using AnonymousStudentReviews.Infrastructure.OpenId;

using OpenIddict.Abstractions;

namespace AnonymousStudentReviews.Api.Configurations;

public static class OpenIddictConfig
{
    public static IServiceCollection AddOpenIddictConfig(this IServiceCollection services,
        WebApplicationBuilder builder)
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

                if (builder.Environment.IsDevelopment())
                {
                    options.AddDevelopmentEncryptionCertificate()
                        .AddDevelopmentSigningCertificate();
                }

                if (!builder.Environment.IsDevelopment())
                {
                    var encryptionCertificateFilePath =
                        builder.Configuration["OPENIDDICT_ENCRYPTION_CERTIFICATE_FILE_CONTAINER_PATH"];

                    var signingCertificateFilePath =
                        builder.Configuration["OPENIDDICT_SIGNING_CERTIFICATE_FILE_CONTAINER_PATH"];

                    if (encryptionCertificateFilePath is null || signingCertificateFilePath is null)
                    {
                        throw new Exception(
                            "OPENIDDICT_ENCRYPTION_CERTIFICATE_FILE_CONTAINER_PATH or OPENIDDICT_SIGNING_CERTIFICATE_FILE_CONTAINER_PATH not set in env");
                    }

                    var encryptionCertificatePassword =
                        builder.Configuration["OPENIDDICT_ENCRYPTION_CERTIFICATE_PASSWORD"];

                    var signingCertificatePassword =
                        builder.Configuration["OPENIDDICT_SIGNING_CERTIFICATE_PASSWORD"];

                    if (encryptionCertificatePassword is null || signingCertificatePassword is null)
                    {
                        throw new Exception(
                            "OPENIDDICT_ENCRYPTION_CERTIFICATE_PASSWORD or OPENIDDICT_SIGNING_CERTIFICATE_PASSWORD not set in env");
                    }

                    var encryptionCert =
                        X509CertificateLoader.LoadPkcs12FromFile(encryptionCertificateFilePath,
                            encryptionCertificatePassword);

                    options.AddEncryptionCertificate(encryptionCert);

                    var signingCert =
                        X509CertificateLoader.LoadPkcs12FromFile(signingCertificateFilePath,
                            signingCertificatePassword);

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
