using AnonymousStudentReviews.Core.Aggregates.User;
using AnonymousStudentReviews.Core.ErrorTypes;

namespace AnonymousStudentReviews.UseCases.Users.Retrieve.RetrieveOne;

public static class RetrieveOneUserErrors
{
    public static readonly NotFoundError NotFound = UserErrors.NotFound;
}
