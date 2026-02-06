using AnonymousStudentReviews.Core.ErrorTypes;

namespace AnonymousStudentReviews.Core.Aggregates.User;

public static class UserErrors
{
    public static readonly NotFoundError NotFound = new("User.NotFound", "User not found");
}
