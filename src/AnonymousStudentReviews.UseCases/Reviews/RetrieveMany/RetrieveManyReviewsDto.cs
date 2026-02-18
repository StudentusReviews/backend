using AnonymousStudentReviews.Core.Abstractions;

namespace AnonymousStudentReviews.UseCases.Reviews.RetrieveMany;

public class RetrieveManyReviewsDto
{
    public int Limit { get; set; } = 20;
    public string? Cursor { get; set; }
    public Guid UniversityId { get; set; }
    public SortOrder SortOrder { get; set; } = SortOrder.Descending;

    // todo: implement review sort by functionality
    // public ReviewSortBy SortBy { get; set; } = ReviewSortBy.Latest;
}
