using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.ErrorTypes;

using Microsoft.AspNetCore.Mvc;

namespace AnonymousStudentReviews.Api.Extensions;

public static class ErrorExtensions
{
    public static ObjectResult ToProblemDetails(this Error error, string instance, string type = "about:blank")
    {
        var httpStatusCode = ErrorTypeToHttpStatusCode(error);

        var problemDetails = new ProblemDetails
        {
            Type = type,
            Title = error.Code,
            Status = httpStatusCode,
            Detail = error.Details,
            Instance = instance
        };

        return new ObjectResult(problemDetails) { StatusCode = problemDetails.Status };
    }

    private static int ErrorTypeToHttpStatusCode(Error error)
    {
        return error switch
        {
            NotFoundError => StatusCodes.Status404NotFound,
            AccessDeniedError => StatusCodes.Status403Forbidden,
            AlreadyExistsError => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status400BadRequest
        };
    }
}
