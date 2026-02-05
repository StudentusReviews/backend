using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.Dummy;
using AnonymousStudentReviews.UseCases.Registration.Abstractions;

namespace AnonymousStudentReviews.UseCases.Dummies.Create;

public class CreateDummyService : ICreateDummyService
{
    private readonly IDummyRepository _dummyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserManager _userManager;

    public CreateDummyService(IDummyRepository dummyRepository, IUnitOfWork unitOfWork, IUserManager userManager)
    {
        _dummyRepository = dummyRepository;
        _unitOfWork = unitOfWork;
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

        if (createDummyResult.IsFailure)
        {
            return createDummyResult;
        }

        _dummyRepository.Create(createDummyResult.Value);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success(createDummyResult.Value);
    }
}
