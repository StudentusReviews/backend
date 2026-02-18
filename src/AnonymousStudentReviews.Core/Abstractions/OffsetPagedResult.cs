namespace AnonymousStudentReviews.Core.Abstractions;

public class OffsetPagedResult<T>
{
    public required IReadOnlyList<T> Items { get; init; }
    public required int TotalCount { get; init; }
    public required int Offset { get; init; }
    public required int Limit { get; init; }
}
