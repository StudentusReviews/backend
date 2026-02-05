using AnonymousStudentReviews.Core.Abstractions;

namespace AnonymousStudentReviews.Core.ErrorTypes;

public record AccessDeniedError(string Code, string Details) : Error(Code, Details)
{
}
