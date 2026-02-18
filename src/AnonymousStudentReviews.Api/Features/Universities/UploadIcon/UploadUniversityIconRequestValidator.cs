using FluentValidation;

namespace AnonymousStudentReviews.Api.Features.Universities.UploadIcon;

public class UploadUniversityIconRequestValidator : AbstractValidator<UploadUniversityIconRequest>
{
    public UploadUniversityIconRequestValidator()
    {
        RuleFor(x => x.File).NotNull();
        RuleFor(x => x.File.Length).GreaterThan(0).LessThanOrEqualTo(5 * 1024 * 1024); // 5MB
    }
}
