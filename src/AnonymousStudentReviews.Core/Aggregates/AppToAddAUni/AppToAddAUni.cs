using AnonymousStudentReviews.Core.Abstractions;
namespace AnonymousStudentReviews.Core.Aggregates.AppToAddAUni;

public class AppToAddAUni
{
    public Guid Id { get; set; }
    public string UniversityName { get; set; }
    public string DomainName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public Guid UserId { get; set; }
    public Guid ApplicationStatusId { get; set; }

    public User.User User { get; set; }
    public AppToAddAUniStatus AppToAddAUniStatus { get; set; }

    public void MarkAsDeleted()
    {
        IsDeleted = true;
    }

    private AppToAddAUni() { }

    public static Result<AppToAddAUni> Create(string universityName, string domainName, Guid userId)
    {
        if (string.IsNullOrWhiteSpace(universityName))
            return Result.Failure<AppToAddAUni>(AppToAddAUniErrors.EmptyUniName);
        if (string.IsNullOrWhiteSpace(domainName))
            return Result.Failure<AppToAddAUni>(AppToAddAUniErrors.EmptyDomainName);
        var application = new AppToAddAUni
        {
            Id = Guid.NewGuid(),
            UniversityName = universityName,
            DomainName = domainName,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsDeleted = false,
            UserId = userId,
            ApplicationStatusId = new AppToAddAUniStatus().Id // Це можна замінити на дефолтний статус, якщо він є
        };
        return Result.Success(application);
    }
}
