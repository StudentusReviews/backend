using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.ApplicationToAddAUniversity.Status;
namespace AnonymousStudentReviews.Core.Aggregates.ApplicationToAddAUniversity.Base;

public class ApplicationToAddAUniversity
{
    public Guid Id { get; set; }
    public string UniversityName { get; set; }
    public string DomainName { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public Guid UserId { get; set; }
    public Guid ApplicationToAddAUniversityStatusId { get; set; }

    public User.User User { get; set; }
    public ApplicationToAddAUniversityStatus ApplicationToAddAUniversityStatus { get; set; }

    public void MarkAsDeleted()
    {
        IsDeleted = true;
    }

    private ApplicationToAddAUniversity() { }

    public static Result<ApplicationToAddAUniversity> Create(string universityName, string domainName, string comment, Guid userId, Guid statusId)
    {
        if (string.IsNullOrWhiteSpace(universityName))
            return Result.Failure<ApplicationToAddAUniversity>(ApplicationToAddAUniversityErrors.EmptyUniversityName);
        if (string.IsNullOrWhiteSpace(domainName))
            return Result.Failure<ApplicationToAddAUniversity>(ApplicationToAddAUniversityErrors.EmptyDomainName);
        if (string.IsNullOrWhiteSpace(comment))
            comment = string.Empty;
        var applicationToAddAUniversity = new ApplicationToAddAUniversity
        {
            Id = Guid.NewGuid(),
            UniversityName = universityName,
            DomainName = domainName,
            Comment = comment,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsDeleted = false,
            UserId = userId,
            ApplicationToAddAUniversityStatusId = statusId
        };
        return Result.Success(applicationToAddAUniversity);
    }
}
