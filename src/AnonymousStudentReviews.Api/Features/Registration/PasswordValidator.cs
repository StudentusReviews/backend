using FluentValidation;

namespace AnonymousStudentReviews.Api.Features.Registration;

public class PasswordValidator : AbstractValidator<string>
{
    public PasswordValidator()
    {
        RuleFor(x => x)
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
