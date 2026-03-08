using AnonymousStudentReviews.Core.ErrorTypes;

namespace AnonymousStudentReviews.Core.Aggregates.University;

public static class UniversityErrors
{
    public static readonly NotFoundError NotFound = new("University.NotFound", "");
}
