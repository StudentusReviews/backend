using AnonymousStudentReviews.Core.Abstractions;

namespace AnonymousStudentReviews.Core.Aggregates.ApplicationToAddAUniversity.Base;

public static class ApplicationToAddAUniversityErrors
{
    public static readonly Error EmptyUniversityName = new("ApplicationToAddAUniniversity.EmptyUniName", "University name cannot be empty.");
    public static readonly Error EmptyDomainName = new("ApplicationToAddAUniniversity.EmptyDomainName", "Domain name cannot be empty.");
    public static Error ApplicationToAddAUniversityNotFound(Guid id) => new("ApplicationToAddAUniversity.ApplicationNotFound", $"Application with id {id} not found.");

}
