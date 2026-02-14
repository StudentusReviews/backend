using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.University;

namespace AnonymousStudentReviews.UseCases.Universities.RetrieveMany;

public class RetrieveManyUniversitiesDto
{
    public int Limit { get; set; } = 20;
    public string? Cursor { get; set; }
    public string? Query { get; set; }
    public string? Name { get; set; }
    public string? City { get; set; }
    public SortBy SortBy { get; set; }
    public SortOrder SortOrder { get; set; }
}
