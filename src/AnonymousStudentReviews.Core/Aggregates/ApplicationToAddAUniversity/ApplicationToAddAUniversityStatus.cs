namespace AnonymousStudentReviews.Core.Aggregates.ApplicationToAddAUniversity;

public class ApplicationToAddAUniversityStatus
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public ICollection<ApplicationToAddAUniversity> AppToAddAUnis { get; set; }
}
