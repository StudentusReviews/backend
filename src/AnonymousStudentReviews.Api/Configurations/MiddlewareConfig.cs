namespace AnonymousStudentReviews.Api.Configurations;

public static class MiddlewareConfig
{
    public static IApplicationBuilder UseAppMiddleware(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseForwardedHeaders();
        }

        app.UseExceptionHandler();

        if (app.Environment.IsDevelopment())
        {
            app.UseOpenApi();
            app.UseSwaggerUi();
            app.MapGet("/", () => Results.Redirect("/swagger"));
        }

        app.UseStaticFiles();

        app.UseCors(ApiConstants.CorsPolicyName);

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}
