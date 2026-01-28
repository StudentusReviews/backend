using AnonymousStudentReviews.Core.Abstractions;

namespace AnonymousStudentReviews.UseCases.AccountVerification;

public static class AccountVerificationErrors
{
    public static readonly Error TokenAlreadyUsed =
        new("AccountVerification.TokenAlreadyUsed", "Token already used");

    public static readonly Error TokenNotFound =
        new("AccountVerification.TokenNotFound", "Token not found");

    public static readonly Error TokenExpired =
        new("AccountVerification.TokenExpired", "Token already used");
}
