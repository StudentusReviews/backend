using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.Dummy;

namespace AnonymousStudentReviews.UseCases.Dummies.Create;

public class CreateDummyService : ICreateDummyService
{
    private readonly IDummyRepository _dummyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateDummyService(IDummyRepository dummyRepository, IUnitOfWork unitOfWork)
    {
        _dummyRepository = dummyRepository;
        _unitOfWork = unitOfWork;
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
