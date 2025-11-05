namespace AnonymousStudentReviews.Api.Configurations;

public static class MiddlewareConfig
{
    public static IApplicationBuilder UseAppMiddleware(this WebApplication app)
    {
        app.UseHttpsRedirection();

        if (app.Environment.IsDevelopment())
        {
            app.UseOpenApi();
            app.UseSwaggerUi();
            app.MapGet("/", () => Results.Redirect("/swagger"));
        }

        app.UseExceptionHandler();

        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}
