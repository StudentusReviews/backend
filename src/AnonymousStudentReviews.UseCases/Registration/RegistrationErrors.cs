using AnonymousStudentReviews.Core.ErrorTypes;

namespace AnonymousStudentReviews.UseCases.Registration;

public static class RegistrationErrors
{
    public static readonly AlreadyExistsError UserAlreadyExists =
        new("Users.Create.UserAlreadyExists", "User with this email already exists");
}
