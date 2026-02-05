using AnonymousStudentReviews.Core.Abstractions;

namespace AnonymousStudentReviews.UseCases.Login;

public interface ILoginService
{
    Task<Result> HandleAsync(LoginDto dto);
}
