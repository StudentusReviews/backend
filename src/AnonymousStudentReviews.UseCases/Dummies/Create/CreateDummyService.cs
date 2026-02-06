using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.Dummy;
using AnonymousStudentReviews.UseCases.Abstractions;
using AnonymousStudentReviews.UseCases.Registration.Abstractions;

namespace AnonymousStudentReviews.UseCases.Dummies.Create;

public class CreateDummyService : ICreateDummyService
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IDummyRepository _dummyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserManager _userManager;

    public CreateDummyService(IDummyRepository dummyRepository, IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService, IUserManager userManager)
    {
        _dummyRepository = dummyRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _userManager = userManager;
    }

    public async Task<Result<Dummy>> ExecuteAsync(CreateDummyDto dto)
    {
        var badCondition = false;
        if (badCondition)
        {
            return Result.Failure<Dummy>(CreateDummyErrors.SomeBadRequestError);
        }

        var createDummyResult = Dummy.Create(dto.Name);

        var userId = _currentUserService.UserId;

        if (userId is null)
        {
            throw new InvalidOperationException("Authentication system is misbehaving");
        }

        var userIdNotNull = userId ?? Guid.Empty;

        var getUserResult = await _userManager.FindByIdAsync(userIdNotNull!);

        if (getUserResult.IsFailure)
        {
            Result.Failure<Dummy>(new Error("", ""));
        }

        var user = getUserResult.Value;

        if (createDummyResult.IsFailure)
        {
            return createDummyResult;
        }

        createDummyResult.Value.User = user;

        _dummyRepository.Create(createDummyResult.Value);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success(createDummyResult.Value);
    }
}
