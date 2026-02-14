namespace AnonymousStudentReviews.Core.Aggregates.ApplicationToAddAUniversity.Status;

public class ApplicationToAddAUniversityStatus
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public ICollection<Base.ApplicationToAddAUniversity> ApplicationToAddAUniversities { get; set; }
}
