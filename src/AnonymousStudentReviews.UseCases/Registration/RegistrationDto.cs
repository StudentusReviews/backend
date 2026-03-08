namespace AnonymousStudentReviews.UseCases.Registration;

public class RegistrationDto
{
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string ReturnUrl { get; init; }
}
