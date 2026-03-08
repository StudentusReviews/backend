using AnonymousStudentReviews.Core.Abstractions;

namespace AnonymousStudentReviews.Core.Aggregates.Dummy;

public static class DummyErrors
{
    public static readonly Error WrongName = new("Dummy.WrongName", "Wrong name");
}
