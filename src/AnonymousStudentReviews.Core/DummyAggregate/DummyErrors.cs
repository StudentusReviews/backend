using AnonymousStudentReviews.Core.Abstractions;

namespace AnonymousStudentReviews.Core.DummyAggregate;

public static class DummyErrors
{
    public static readonly Error WrongName = new("Dummy.WrongName", "Wrong name");
}
