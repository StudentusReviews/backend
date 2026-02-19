using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.User;

namespace AnonymousStudentReviews.UseCases.Users.Retrieve.RetrieveOne;

public class RetrieveOneUserService : IRetrieveOneUserService
{
    private readonly IUserRepository _userRepository;

    public RetrieveOneUserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<User>> HandleAsync(Guid userId)
    {
        var findUserResult = await _userRepository.FindByIdAsync(userId);

        if (findUserResult.IsFailure)
        {
            return Result.Failure<User>(RetrieveOneUserErrors.NotFound);
        }

        return findUserResult.Value;
    }
}
