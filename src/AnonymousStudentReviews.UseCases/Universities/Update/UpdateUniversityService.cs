using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.University;

namespace AnonymousStudentReviews.UseCases.Universities.Update;

public class UpdateUniversityService : IUpdateUniversityService
{
    private readonly IUniversityRepository _repository;

    public UpdateUniversityService(IUniversityRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<University>> ExecuteAsync(UpdateUniversityDto dto)
    {
        var getResult = await _repository.FindByIdAsync(dto.UniversityId);

        if (getResult.IsFailure)
        {
            return Result.Failure<University>(getResult.Error);
        }

        var university = getResult.Value;

        university.Name = dto.Name;
        university.City = dto.City;
        university.Website = dto.Website;
        university.Description = dto.Description;

        var updateResult = await _repository.UpdateAsync(university);

        return updateResult;
    }
}
