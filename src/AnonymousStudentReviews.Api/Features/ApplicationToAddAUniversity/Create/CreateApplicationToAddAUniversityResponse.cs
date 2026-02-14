namespace AnonymousStudentReviews.Api.Features.ApplicationToAddAUniversity.Create;

public class CreateApplicationToAddAUniversityResponse
{
    public Guid Id { get; init; }
    public string UniversityName { get; init; }
    public string DomainName { get; init; }
    public DateTime CreatedAt { get; init; }
    public Guid UserId { get; init; }
}
