using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.User;
using AnonymousStudentReviews.Core.ErrorTypes;

namespace AnonymousStudentReviews.UseCases.Users.Edit;

public static class EditUserErrors
{
    public static readonly NotFoundError NotFound = UserErrors.NotFound;
    public static readonly AccessDeniedError AccessDenied = new("Users.Edit.AccessDenied", "Access denied");
    public static readonly Error NoChangedDetected = new("Users.Edit.NoChangedDetected", "Nothing to edit");
}
