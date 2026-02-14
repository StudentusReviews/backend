using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.ErrorTypes;

namespace AnonymousStudentReviews.Core.Aggregates.Review;

public static class ReviewErrors
{
    public static readonly Error InvalidIds = new Error("review.invalid_ids", "UniversityId and UserId must be valid.");
    public static readonly Error InvalidScore = new Error("review.invalid_score", "Score must be between 1 and 10.");
    public static readonly Error InvalidBodyLength = new Error("review.invalid_body_length", "Body length must be between 1 and 4000 characters.");

    public static readonly Error NotFound = new NotFoundError("review.not_found", "Review was not found.");
    public static readonly Error AccessDenied = new AccessDeniedError("review.access_denied", "You do not have access to this review.");
    public static readonly Error UniversityMismatch = new AccessDeniedError("review.university_mismatch", "You can create reviews only for your university.");
    public static readonly Error UniversityNotSet = new Error("review.university_not_set", "User has no university assigned.");
}
