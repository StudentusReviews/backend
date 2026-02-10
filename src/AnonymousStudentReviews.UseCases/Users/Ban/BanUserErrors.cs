using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.ErrorTypes;

namespace AnonymousStudentReviews.UseCases.Users.Ban;

public static class BanUserErrors
{
    public static readonly Error SelfBan = new("Users.Ban.SelfBan", "Can't ban yourself");
    public static readonly Error UserNotFound = new("Users.Ban.UserNotFound", "User to be banned not found");

    public static readonly AccessDeniedError UserIsAdmin = new("Users.Ban.UserIsAdmin",
        "User to be banned has an admin role");
}
