using AnonymousStudentReviews.Core.ErrorTypes;

namespace AnonymousStudentReviews.UseCases.Users.Create;

public static class CreateUserErrors
{
    public static readonly AlreadyExistsError UserAlreadyExists =
        new("Users.Create.UserAlreadyExists", "User with this email already exists");
}
