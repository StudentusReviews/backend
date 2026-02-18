using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.ErrorTypes;

namespace AnonymousStudentReviews.Core.Aggregates.ApplicationToAddAUniversity.Base;

public static class ApplicationToAddAUniversityErrors
{
    public static readonly Error EmptyUniversityName = new("ApplicationToAddAUniniversity.EmptyUniName", "University name cannot be empty.");
    public static readonly Error EmptyDomainName = new("ApplicationToAddAUniniversity.EmptyDomainName", "Domain name cannot be empty.");
    public static NotFoundError ApplicationToAddAUniversityNotFound(Guid id) => new("ApplicationToAddAUniversity.ApplicationNotFound", $"Application with id {id} not found.");
    public static AccessDeniedError ApplicationToAddAUniversityInaccessible(Guid appId, Guid userId) => new("ApplicationToAddAUniversity.ApplicationInaccessible", $"Application with id {appId} is not accessible to user with id {userId}.");

}
