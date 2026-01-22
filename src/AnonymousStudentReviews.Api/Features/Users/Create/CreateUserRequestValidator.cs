using FluentValidation;

namespace AnonymousStudentReviews.Api.Features.Users.Create;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("The email can't be empty")
            .NotNull().WithMessage("The email can't be null")
            .EmailAddress().WithMessage("The email must be in the email format")
            .MaximumLength(70).WithMessage("The email's length must not exceed 70 characters");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .NotNull()
            .WithMessage("Password can't be null")
            .MinimumLength(12)
            .WithMessage("Password must be at least 12 characters long.")
            .MaximumLength(128)
            .WithMessage("Password must not exceed 128 characters.")
            .Matches(@"[A-Z]")
            .WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]")
            .WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"\d")
            .WithMessage("Password must contain at least one number.")
            .Matches(@"[^\w\d\s]")
            .WithMessage("Password must contain at least one special character.");
    }
}
