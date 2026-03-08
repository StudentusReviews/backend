namespace AnonymousStudentReviews.Core.Abstractions;

public class CursorPagedResult<T>
{
    public CursorPagedResult(List<T> data, string? nextCursor, bool hasNextPage)
    {
        Data = data;
        NextCursor = nextCursor;
        HasNextPage = hasNextPage;
    }

    public List<T> Data { get; set; }

    public string? NextCursor { get; set; }
    public bool HasNextPage { get; set; }
}
