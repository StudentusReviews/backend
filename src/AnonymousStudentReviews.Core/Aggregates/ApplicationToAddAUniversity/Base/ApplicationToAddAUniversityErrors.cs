using AnonymousStudentReviews.Core.Abstractions;

namespace AnonymousStudentReviews.Core.Aggregates.ApplicationToAddAUniversity.Base;

public static class ApplicationToAddAUniversityErrors
{
    public static readonly Error EmptyUniName = new("ApplicationToAddAUniniversity.EmptyUniName", "University name cannot be empty.");
    public static readonly Error EmptyDomainName = new("ApplicationToAddAUniniversity.EmptyDomainName", "Domain name cannot be empty.");
}
