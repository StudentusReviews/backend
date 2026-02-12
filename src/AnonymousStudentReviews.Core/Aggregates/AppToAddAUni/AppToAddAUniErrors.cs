using AnonymousStudentReviews.Core.Abstractions;

namespace AnonymousStudentReviews.Core.Aggregates.AppToAddAUni;

public static class AppToAddAUniErrors
{
    public static readonly Error EmptyUniName = new("AppToAddAUni.EmptyUniName", "University name cannot be empty.");
    public static readonly Error EmptyDomainName = new("AppToAddAUni.EmptyDomainName", "Domain name cannot be empty.");
}
