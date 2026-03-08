using System.ComponentModel.DataAnnotations;

using Microsoft.Extensions.Options;

namespace AnonymousStudentReviews.Api.Extensions;

public static class ConfigurationExtensions
{
    public static T GetValidated<T>(this IConfiguration configuration, string sectionName) where T : class, new()
    {
        var options = new T();
        configuration.GetSection(sectionName).Bind(options);

        var validationContext = new ValidationContext(options);
        var validationResults = new List<ValidationResult>();

        if (!Validator.TryValidateObject(options, validationContext, validationResults, true))
        {
            var errors = validationResults
                .Select(r => r.ErrorMessage ?? $"Validation failed for {typeof(T).Name}.")
                .ToList();

            throw new OptionsValidationException(sectionName, typeof(T), errors);
        }

        return options;
    }
}
