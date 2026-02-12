using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.User;
using AnonymousStudentReviews.UseCases.Registration.Abstractions;

namespace AnonymousStudentReviews.UseCases.Users.Retrieve;

public class RetrieveUsersService : IRetrieveUsersService
{
    private readonly IEmailHasher _emailHasher;
    private readonly IUserRepository _userRepository;

    public RetrieveUsersService(IUserRepository userRepository, IEmailHasher emailHasher)
    {
        _userRepository = userRepository;
        _emailHasher = emailHasher;
    }

    public async Task<Result<PaginatedList<UserPreview>>> HandleAsync(RetrieveUsersDto dto)
    {
        var emailHash = dto.Email is not null ? _emailHasher.Hash(dto.Email) : null;

        var result = await _userRepository.GetAllAsync(
            dto.QueryString, dto.UserId, dto.UniversityId, dto.UniversityName,
            emailHash, dto.SortBy, dto.SortOrder, dto.PageNumber, dto.PageSize);

        return Result.Success(result);
    }
}
