namespace AnonymousStudentReviews.Core.Abstractions;

public abstract class EntityBase<TId>
{
    protected EntityBase(TId id)
    {
        Id = id;
    }

    protected EntityBase()
    {
    }

    public TId Id { get; init; }
}
