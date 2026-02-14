namespace AnonymousStudentReviews.Core.Aggregates.AppToAddAUni;

public class AppToAddAUniStatus
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public ICollection<AppToAddAUni> AppToAddAUnis { get; set; }
}
