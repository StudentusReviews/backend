using FluentValidation;

namespace AnonymousStudentReviews.Api.Features.Users.Retrieve.RetrieveMany;

public class RetrieveManyUsersQueryParametersValidator : AbstractValidator<RetrieveManyUsersQueryParameters>
{
    public RetrieveManyUsersQueryParametersValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);
    }
}
