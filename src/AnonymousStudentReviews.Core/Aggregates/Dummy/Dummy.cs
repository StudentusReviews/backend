using AnonymousStudentReviews.Core.Abstractions;

namespace AnonymousStudentReviews.Core.Aggregates.Dummy;

public class Dummy : EntityBase<long>, IAggregateRoot
{
    private Dummy(string name)
    {
        Name = name;
    }

    private Dummy()
    {
    }

    public string Name { get; private set; }
    public Guid UserId { get; set; }

    public User.User User { get; set; }

    public static Result<Dummy> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name) || name == "some wrong name according to business rules")
        {
            return Result.Failure<Dummy>(DummyErrors.WrongName);
        }

        return new Dummy(name);
    }

    public Result<Dummy> UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName) || newName == "some wrong name according to business rules")
        {
            return Result.Failure<Dummy>(DummyErrors.WrongName);
        }

        Name = newName;
        return this;
    }
}
