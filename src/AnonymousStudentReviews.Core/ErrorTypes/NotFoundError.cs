using AnonymousStudentReviews.Core.Abstractions;

namespace AnonymousStudentReviews.Core.ErrorTypes;

public record NotFoundError(string Code, string Details) : Error(Code, Details)
{
}
