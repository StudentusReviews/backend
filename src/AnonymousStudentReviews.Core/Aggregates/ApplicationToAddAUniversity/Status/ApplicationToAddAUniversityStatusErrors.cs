using AnonymousStudentReviews.Core.ErrorTypes;

namespace AnonymousStudentReviews.Core.Aggregates.ApplicationToAddAUniversity.Status;

public class ApplicationToAddAUniversityStatusErrors
{
    public static readonly NotFoundError StatusNotFound = new("ApplicationToAddAUniversityStatus.NotFound", "Status not found");
}
