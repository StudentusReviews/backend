using AnonymousStudentReviews.Core.Abstractions;

namespace AnonymousStudentReviews.UseCases.Users.Edit;

public interface IEditUserService
{
    Task<Result> HandleAsync(EditUserDto dto);
}
