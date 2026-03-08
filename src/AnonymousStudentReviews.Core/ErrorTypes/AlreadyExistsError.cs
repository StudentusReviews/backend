using AnonymousStudentReviews.Core.Abstractions;

namespace AnonymousStudentReviews.Core.ErrorTypes;

public record AlreadyExistsError(string Code, string Details) : Error(Code, Details)
{
}
