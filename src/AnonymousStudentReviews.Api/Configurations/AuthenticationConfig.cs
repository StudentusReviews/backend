using Microsoft.AspNetCore.Authentication.Cookies;

namespace AnonymousStudentReviews.Api.Configurations;

public static class AuthenticationConfig
{
    public static IServiceCollection AddAuthenticationConfig(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.LoginPath = "/api/login";
                options.LogoutPath = "/api/logout";
                options.ReturnUrlParameter = "return-url";
            });


        return serviceCollection;
    }
}
