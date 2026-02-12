namespace AnonymousStudentReviews.Api.Features.AppToAddAUni.Create;

public class CreateAppToAddAUniResponse
{
    public Guid Id { get; init; }
    public string UniversityName { get; init; }
    public string DomainName { get; init; }
    public DateTime CreatedAt { get; init; }
    public Guid UserId { get; init; }
}
