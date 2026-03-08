namespace AnonymousStudentReviews.Core.Abstractions;

public record Error(string Code, string Details)
{
    public static readonly Error None = new(string.Empty, string.Empty);

    public static readonly Error NullValue = new("Error.NullValue", "Null value was provided");

    public string Code { get; } = Code;
    public string Details { get; } = Details;
}
