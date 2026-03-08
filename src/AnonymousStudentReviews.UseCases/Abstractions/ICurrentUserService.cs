namespace AnonymousStudentReviews.UseCases.Abstractions;

public interface ICurrentUserService
{
    bool IsAuthenticated { get; }
    Guid? UserId { get; }
}
