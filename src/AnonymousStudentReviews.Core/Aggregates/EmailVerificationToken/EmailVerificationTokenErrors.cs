using AnonymousStudentReviews.Core.ErrorTypes;

namespace AnonymousStudentReviews.Core.Aggregates.EmailVerificationToken;

public static class EmailVerificationTokenErrors
{
    public static readonly NotFoundError NotFound = new("EmailVerificationToken.NotFound", "Not found");
}
