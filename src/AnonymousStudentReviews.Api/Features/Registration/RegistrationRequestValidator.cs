using FluentValidation;

namespace AnonymousStudentReviews.Api.Features.Registration;

public class RegistrationRequestValidator : AbstractValidator<RegistrationRequest>
{
    public RegistrationRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("The email can't be empty")
            .NotNull().WithMessage("The email can't be null")
            .EmailAddress().WithMessage("The email must be in the email format")
            .MaximumLength(70).WithMessage("The email's length must not exceed 70 characters");

        RuleFor(x => x.Password)
            .SetValidator(new PasswordValidator());

        RuleFor(x => x.ConfirmPassword)
            .SetValidator(new PasswordValidator())
            .Equal(x => x.Password)
            .WithMessage("Passwords do not match");
    }
}
