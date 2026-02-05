using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace AnonymousStudentReviews.Api.Configurations;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(
            exception, "Exception occurred: {Message}", exception.Message);

        var problemDetails = new ProblemDetails
        {
            Type = "about:blank",
            Title = "Internal server error.",
            Status = StatusCodes.Status500InternalServerError,
            Instance = httpContext.Request.Path,
        };

        if (httpContext.Request.Headers.Accept.Contains("application/json", StringComparer.OrdinalIgnoreCase))
        {

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response
                .WriteAsJsonAsync(problemDetails, cancellationToken);
        }
        else
        {
            var viewResult = new ViewResult
            {
                ViewName = "Error",
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await viewResult.ExecuteResultAsync(
                new ActionContext
                {
                    ActionDescriptor = new ActionDescriptor(),
                    HttpContext = httpContext,
                    RouteData = httpContext.GetRouteData()
                });
        }



        return true;
    }
}
