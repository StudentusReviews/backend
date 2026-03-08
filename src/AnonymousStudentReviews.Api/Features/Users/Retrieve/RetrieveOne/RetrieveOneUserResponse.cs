using AnonymousStudentReviews.Core.Aggregates.Role;

namespace AnonymousStudentReviews.Api.Features.Users.Retrieve.RetrieveOne;

public class RetrieveOneUserResponse
{
    public Guid Id { get; set; }
    public Guid? UniversityId { get; set; }
    public DateTime RegistrationDate { get; set; }
    public bool EmailConfirmed { get; set; }
    public bool IsBanned { get; set; }
    public int AccessFailedCount { get; set; }
    public DateTimeOffset? LockoutEnd { get; set; }
    public IEnumerable<RolePreview> Roles { get; set; }
    public string? UniversityName { get; set; }
}
