using FluentValidation;

namespace AnonymousStudentReviews.Api.Extensions;

public static class CustomValidators
{
    public static IRuleBuilderOptions<T, string> IsUri<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(x => StringIsUri(x));
    }

    public static IRuleBuilderOptions<T, string> IsRelativeUri<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(x => StringIsUri(x, UriKind.Relative));
    }

    private static bool StringIsUri(string value, UriKind uriKind = UriKind.RelativeOrAbsolute)
    {
        var isUri = Uri.TryCreate(value, uriKind, out var uriResult);

        if (!isUri || uriResult is null)
        {
            return false;
        }

        if (uriResult.IsAbsoluteUri &&
             uriResult.Scheme != Uri.UriSchemeHttp || uriResult.Scheme != Uri.UriSchemeHttps)
        {
            return false;
        }

        return true;
    }
}
