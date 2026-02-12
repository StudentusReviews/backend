using FluentValidation;

namespace AnonymousStudentReviews.Api.Features.Users.Retrieve;

public class RetrieveUserQueryParametersValidator : AbstractValidator<RetrieveUsersQueryParameters>
{
    public RetrieveUserQueryParametersValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);
    }
}
