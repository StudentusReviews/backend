using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.University;

namespace AnonymousStudentReviews.UseCases.Universities.Create;

public class CreateUniversityService : ICreateUniversityService
{
    private readonly IUniversityRepository _repository;

    public CreateUniversityService(IUniversityRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<University>> ExecuteAsync(CreateUniversityDto dto)
    {
        var university = new University
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            City = dto.City,
            Website = dto.Website,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(university);

        return Result.Success(university);
    }
}
