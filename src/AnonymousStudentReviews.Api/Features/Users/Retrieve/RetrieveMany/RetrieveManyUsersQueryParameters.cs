using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.User;

namespace AnonymousStudentReviews.Api.Features.Users.Retrieve.RetrieveMany;

public class RetrieveManyUsersQueryParameters
{
    public string? QueryString { get; set; }
    public Guid? UserId { get; set; }
    public Guid? UniversityId { get; set; }
    public string? UniversityName { get; set; }
    public string? Email { get; set; }
    public SortBy SortBy { get; set; } = SortBy.UniversityName;
    public SortOrder SortOrder { get; set; } = SortOrder.Ascending;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
