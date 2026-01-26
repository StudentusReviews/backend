using FluentValidation.Results;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AnonymousStudentReviews.Api.Extensions;

public static class ValidationResultExtensions
{
    public static ObjectResult ToProblemDetails(this ValidationResult validationResult, string instance,
        string type = "about:blank")
    {
        var problemDetails = new ProblemDetails
        {
            Type = type,
            Title = "Validation error",
            Status = StatusCodes.Status400BadRequest,
            Detail = "One or more validation errors have occured.",
            Instance = instance,
            Extensions = { { "errors", validationResult.ToDictionary() } }
        };

        return new ObjectResult(problemDetails) { StatusCode = StatusCodes.Status400BadRequest };
    }

    public static void AddToModelState(this ValidationResult result, ModelStateDictionary modelState)
    {
        foreach (var error in result.Errors)
        {
            modelState.AddModelError(error.PropertyName, error.ErrorMessage);
        }
    }
}
