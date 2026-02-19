namespace AnonymousStudentReviews.UseCases.Users.Edit;

public class EditUserDto
{
    public Guid UserId { get; set; }
    public bool IsBanned { get; set; }
}
