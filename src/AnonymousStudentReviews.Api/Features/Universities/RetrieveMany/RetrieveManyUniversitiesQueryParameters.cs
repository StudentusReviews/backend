using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.University;

namespace AnonymousStudentReviews.Api.Features.Universities.RetrieveMany;

public class RetrieveManyUniversitiesQueryParameters
{
    public int Limit { get; set; } = 20;
    public int Offset { get; set; } = 0;

    public string? Query { get; set; }
    public string? Name { get; set; }
    public string? City { get; set; }

    public UniversitySortBy SortBy { get; set; } = UniversitySortBy.Rating;

    public SortOrder SortOrder { get; set; } = SortOrder.Descending;
}
