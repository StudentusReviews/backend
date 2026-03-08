using AnonymousStudentReviews.Core.Abstractions;

namespace AnonymousStudentReviews.UseCases.Login;

public static class LoginErrors
{
    public static readonly Error WrongPassword = new("Login.WrongPassword", "Wrong password");
    public static readonly Error WrongEmailOrPassword = new("Login.WrongEmailOrPassword", "Wrong email or password");
}
