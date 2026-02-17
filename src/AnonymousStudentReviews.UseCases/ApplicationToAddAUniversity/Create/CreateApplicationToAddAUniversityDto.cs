namespace AnonymousStudentReviews.UseCases.ApplicationToAddAUniversity.Create;

public class CreateApplicationToAddAUniversityDto
{
    public string UniversityName { get; set; }
    public string DomainName { get; set; }
    public string? Comment { get; set; }
}
