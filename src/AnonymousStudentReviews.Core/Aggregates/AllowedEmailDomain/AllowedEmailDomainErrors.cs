using AnonymousStudentReviews.Core.ErrorTypes;

namespace AnonymousStudentReviews.Core.Aggregates.AllowedEmailDomain;

public static class AllowedEmailDomainErrors
{
    public static readonly NotFoundError NotFound =
        new("AllowedEmailDomain.Notfound", "Not found");
}
